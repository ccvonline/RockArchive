using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using church.ccv.CCVCore.PushNotification.Model;
using Rock;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;
using static church.ccv.CCVCore.PushNotification.Util.PushNotificationConstants;
using static GfbHttpSender;

//****NOTE**** For the required encryption to work, you must set "Load User Profile=true" on the Application Pool instance that runs Rock.
// Otherwise, user profile data is not loaded, and the required encryption keys can't be used.
// If you don't do this, you will get the following exception: CryptographicException was unhandled: System cannot find the specified file
//****END NOTE****

namespace church.ccv.CCVCore.PushNotification.Util
{
    public class PushNotificationService
    {
        /// <summary>
        /// Saves a device, platform and optionally an associated person to the table
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="platform"></param>
        /// <param name="personAliasId"></param>
        /// <returns></returns>
        public static void SaveDevice( string deviceId, string platform, int? personAliasId )
        {
            RockContext rockContext = new RockContext();
            Service<PushNotificationDevice> pnService = new Service<PushNotificationDevice>( rockContext );

            // first see if we already have this device in our table
            var deviceRecord = pnService.Queryable().Where( r => r.DeviceId == deviceId ).SingleOrDefault();
            if ( deviceRecord != null )
            {
                // Update the person alias id to whatever the new value is, even if that's null.
                // That would imply a user logged out on their device, in which case we shouldn't
                // assume it's their identity anymore.
                deviceRecord.PersonAliasId = personAliasId;
            }
            else
            {
                deviceRecord = new PushNotificationDevice
                {
                    DeviceId = deviceId,
                    Platform = platform,
                    PersonAliasId = personAliasId
                };
                pnService.Add( deviceRecord );
            }

            // stamp now as our updated date time. This is important so we can run a cleanup job for abandoned deviceIds
            // (which would happen when a user's device Id changes due to a new phone, reinstall of the app, etc.)
            deviceRecord.LastSeenDateTime = DateTime.Now;
            
            rockContext.SaveChanges();
        }

        /// <summary>
        /// Gets the person Ids for every device that is associated with a person
        /// </summary>
        /// <returns></returns>
        public static IQueryable<int> GetKnownDevicePersonIds()
        {
            // return a list of personAliasIds for devices associated with people
            RockContext rockContext = new RockContext();
            Service<PushNotificationDevice> pnService = new Service<PushNotificationDevice>( rockContext );

            // first see if we already have this device in our table
            return pnService.Queryable().Where( r => r.PersonAliasId != null ).Select( r => r.PersonAlias.PersonId ).Distinct();
        }

        /// <summary>
        /// Sends a push notification with an optionally custom title and message to the provided list of people
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="personIds"></param>
        public static void SendPushNotification( string title, string message, List<int> personIds, Dictionary<string, string> metaDataValue = null )
        {
            RockContext rockContext = new RockContext();
            var deviceQuery = new Service<PushNotificationDevice>( rockContext ).Queryable();

            // get only devices tied to the personIds provided
            var filteredDeviceIdList = deviceQuery.Where( d => personIds.Contains( d.PersonAlias.PersonId ) ).Select( d => d.Id ).ToList();
            var filteredDeviceList = LoadAndUpdateDevices( filteredDeviceIdList );

            // build a list of personalized push data objects
            List<PushNotificationData> pnDataList = new List<PushNotificationData>();
            foreach ( var device in filteredDeviceList )
            {
                // setup lava merge fields
                Dictionary<string, object> mergeFields = new Dictionary<string, object>
                {
                    { "Person", device.PersonAlias.Person }
                };

                PushNotificationData pnData = new PushNotificationData();
                pnData.Title = title.ResolveMergeFields( mergeFields );
                pnData.Message = message.ResolveMergeFields( mergeFields );
                pnData.DeviceId = device.DeviceId;
                pnData.Platform = device.Platform;

                pnDataList.Add( pnData );
            }

            SendApplePushNotification( pnDataList, metaDataValue );
            SendGooglePushNotification( pnDataList, metaDataValue );
        }

        /// <summary>
        /// Sends a push notification with a simple title and message to all devices
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        public static void SendPushNotification( string title, string message, Dictionary<string, string> metaDataValue = null )
        {
            RockContext rockContext = new RockContext();
            var deviceIdList = new Service<PushNotificationDevice>( rockContext ).Queryable().Select( d => d.Id ).ToList();
            var deviceList = LoadAndUpdateDevices( deviceIdList );

            // build a list of push data objects (since no person was defined, use the same message for everyone)
            List<PushNotificationData> pnDataList = new List<PushNotificationData>();
            foreach ( var device in deviceList )
            {
                PushNotificationData pnData = new PushNotificationData();
                pnData.Title = title;
                pnData.Message = message;
                pnData.DeviceId = device.DeviceId;
                pnData.Platform = device.Platform;

                pnDataList.Add( pnData );
            }

            SendApplePushNotification( pnDataList, metaDataValue );
            SendGooglePushNotification( pnDataList, metaDataValue );
        }

        private static List<PushNotificationDevice> LoadAndUpdateDevices( List<int> deviceIdList )
        {
            // this will return a list of device ids that can be pushed to,
            // but before returning them, update their "LastPushed" value so that we know how recently we pushed
            // a notification to them. 

            // This allows us to do filtering and make sure we never spam a device with too many notifications in a given
            // timeframe.
            List<PushNotificationDevice> deviceList = new List<PushNotificationDevice>();

            // load each one in its own context so we don't bloat the context with our changes
            foreach ( var deviceId in deviceIdList )
            {
                RockContext updateContext = new RockContext();
                var device = new Service<PushNotificationDevice>( updateContext ).Get( deviceId );

                device.LastPushedDateTime = DateTime.Now;

                updateContext.SaveChanges();

                deviceList.Add( device );
            }

            return deviceList;
        }

        // wrapper class defining the per-device package to push to the device.
        // This allows us to customize the message with Lava
        private class PushNotificationData
        {
            public string Title;
            public string Message;
            public string DeviceId;
            public string Platform;

            //Used to store platform specific formatted data (like the json blob for iOS)
            public object PlatformBlob; 
        }

        /// <summary>
        /// Prepares the data on a per-device basis and sends it on a worker thread
        /// </summary>
        /// <param name="pnDataList"></param>
        /// <param name="metaDataValue"></param>
        private static void SendGooglePushNotification( List<PushNotificationData> pnDataList, Dictionary<string, string> metaDataValue = null )
        {
            // take just Android devices
            pnDataList = pnDataList.Where( d => d.Platform == Platform.Android ).ToList();
            
            var googleSettings = GlobalAttributesCache.Value( "GoogleFirebaseCloudMessagingApiKey" );

            System.Threading.Tasks.Task.Run( async () =>
            {                
                try
                {
                    GfbHttpSender gfbSender = new GfbHttpSender( googleSettings );

                    List<Task<GfbSendResult>> awaitingTasks = new List<Task<GfbSendResult>>();
                    foreach ( var pnData in pnDataList )
                    {
                        // package up the push data
                        var jsonBlob = new
                        {
                            to = pnData.DeviceId,

                            notification = new
                            {
                                title = pnData.Title,
                                body = pnData.Message
                            },

                            data = new
                            {
                                metaData = metaDataValue
                            }
                        };

                        // send it
                        Task<GfbSendResult> task = gfbSender.SendAsync( jsonBlob );
                        awaitingTasks.Add( task );
                    }

                    // wait for all requests to complete
                    foreach ( Task<GfbSendResult> task in awaitingTasks )
                    {
                        await task;
                        if ( task.Result.Success != true && task.Result.Exception != null )
                        {
                            ExceptionLogService.LogException( task.Result.Exception );

                            // also log the result of the send attempt
                            string exceptionMessage = string.Format( "GFB Failed: Status: {0}", task.Result.Status );
                            Exception exception = new Exception( exceptionMessage );
                            ExceptionLogService.LogException( exception );
                        }
                    }
                }
                catch ( Exception ex )
                {
                    ExceptionLogService.LogException( ex );
                }
            } );
        }

        /// <summary>
        /// Prepares the data on a per-device basis and sends it on a worker thread
        /// </summary>
        /// <param name="pnDataList"></param>
        private static void SendApplePushNotification( List<PushNotificationData> pnDataList, Dictionary<string, string> metaDataValue = null )
        {
            // take just iOS devices
            pnDataList = pnDataList.Where( d => d.Platform == Platform.iOS ).ToList();

            string p8PrivateKey, p8PrivateKeyId, teamId, appBundleIdentifier;
            ApnServerType serverType;
            GetAPNValues( out p8PrivateKey, out p8PrivateKeyId, out teamId, out appBundleIdentifier, out serverType );

            // build the json blob specific to each device
            foreach ( var notificationObj in pnDataList )
            {
                var jsonBlob = new
                {
                    aps = new
                    {
                        alert = new
                        {
                            title = notificationObj.Title,
                            body = notificationObj.Message
                        }
                    },

                    metaData = metaDataValue
                };

                notificationObj.PlatformBlob = jsonBlob;
            }

            System.Threading.Tasks.Task.Run( async () =>
            {
                try
                {
                    await InvokeAsyncAPNSend( p8PrivateKey,
                                              p8PrivateKeyId,
                                              teamId,
                                              appBundleIdentifier,
                                              serverType,
                                              pnDataList );
                }
                catch ( Exception ex )
                {
                    ExceptionLogService.LogException( ex );
                }
            } );
        }

        /// <summary>
        /// Actually sends the Apple Push Notification vis Reflection
        /// </summary>
        /// <param name="p8PrivateKey"></param>
        /// <param name="p8PrivateKeyId"></param>
        /// <param name="teamId"></param>
        /// <param name="appBundleIdentifier"></param>
        /// <param name="serverType"></param>
        /// <param name="pnDataList"></param>
        /// <returns></returns>
        private async static Task InvokeAsyncAPNSend( string p8PrivateKey, 
                                                      string p8PrivateKeyId, 
                                                      string teamId, 
                                                      string appBundleIdentifier,
                                                      ApnServerType serverType,
                                                      List<PushNotificationData> pnDataList )
        {
            Assembly apnSenderDLL = Assembly.Load( APNSenderAssembly.Name );

            // get the class type for the ApnHttp2Sender
            Type apnHttp2SenderType = apnSenderDLL.GetType( APNSenderAssembly.ClassType );

            // grab its constructor
            ConstructorInfo primaryConstructor = apnHttp2SenderType.GetConstructors()[0];

            // create an instance of the apnHttp2Sender invoking its main constructor.
            // This is essentially the same as:
            // Ex: ApnHttp2Sender apnSenderInstanceObj = new ApnHttp2Sender( p8PrivateKey, p8PrivateKeyId, teamId, appBundleIdentifier, serverType );
            var apnSenderInstanceObj = primaryConstructor.Invoke( new object[] { new System.String( p8PrivateKey.ToCharArray() ),
                                                                                 new System.String( p8PrivateKeyId.ToCharArray() ),
                                                                                 new System.String( teamId.ToCharArray() ),
                                                                                 new System.String( appBundleIdentifier.ToCharArray() ),
                                                                                 serverType } );

            // Get the SendAsync method
            var method = apnHttp2SenderType.GetMethod( APNSenderAssembly.SendAsyncMethod );

            List<Task> awaitingTasks = new List<Task>();
            foreach ( var pnData in pnDataList )
            {
                // invoke the method. This is the same as:
                // Ex: Task task = apnSender.SendAsync( deviceId, message, null, "0", "10" );
                Task task = (Task) method.Invoke( apnSenderInstanceObj, new object[] { pnData.DeviceId, pnData.PlatformBlob, null, "0", "10" } );
                awaitingTasks.Add( task );
            }

            // wait for all requests to complete
            foreach ( Task task in awaitingTasks )
            {
                await task;

                // look at the result from the send attempt (This object is defined in church.ccv.ApnHttp2Sender)
                var resultProp = task.GetType().GetProperty( "Result" );
                var resultObj = resultProp.GetValue( task );

                var exceptionVal = resultObj.GetPropertyValue( "Exception" ) as Exception;
                var statusVal = (HttpStatusCode) resultObj.GetPropertyValue( "Status" );
                var successVal = (bool) resultObj.GetPropertyValue( "Success" );
                var notificationVal = resultObj.GetPropertyValue( "NotificationId" ) as string;

                if ( successVal != true && exceptionVal != null )
                {
                    // log the exception
                    ExceptionLogService.LogException( exceptionVal );

                    // also log the result of the send attempt
                    string exceptionMessage = string.Format( "APN Failed: Status: {0} Notification Id: {1}", statusVal, notificationVal );
                    Exception exception = new Exception( exceptionMessage );
                    ExceptionLogService.LogException( exception );
                }
            }

            // Get the Dispose method and invoke it.
            // This is the same as:
            // Ex: apnSender.Dispose();
            var disposeMethod = apnHttp2SenderType.GetMethod( APNSenderAssembly.DisposeMethod );
            disposeMethod.Invoke( apnSenderInstanceObj, null );
        }

        /// <summary>
        /// Returns the values required for signing push notification requests that go to Apple
        /// </summary>
        /// <param name="p8PrivateKey"></param>
        /// <param name="p8PrivateKeyId"></param>
        /// <param name="teamId"></param>
        /// <param name="appBundleIdentifier"></param>
        /// <param name="serverType"></param>
        private static void GetAPNValues( out string p8PrivateKey, out string p8PrivateKeyId, out string teamId, out string appBundleIdentifier, out ApnServerType serverType )
        {
            // Gets all the values required for sending an Apple Push Notification
            var apnSettings = GlobalAttributesCache.Value( APNMatrixKeys.APNMatrixKey );
            var attributeMatrix = new AttributeMatrixService( new Rock.Data.RockContext() ).Get( new Guid( apnSettings ) );

            var attributeMatrixItem = attributeMatrix.AttributeMatrixItems.FirstOrDefault();
            attributeMatrixItem.LoadAttributes();

            p8PrivateKey = attributeMatrixItem.AttributeValues[APNMatrixKeys.P8PrivateKey ].ToString();
            p8PrivateKeyId = attributeMatrixItem.AttributeValues[APNMatrixKeys.P8PrivateKeyId].ToString();
            teamId = attributeMatrixItem.AttributeValues[APNMatrixKeys.TeamId].ToString();
            appBundleIdentifier = attributeMatrixItem.AttributeValues[APNMatrixKeys.AppBundleIdentifier].ToString();
            serverType = ( ApnServerType ) Enum.Parse( typeof( ApnServerType ), attributeMatrixItem.AttributeValues[APNMatrixKeys.ApnServerType].ToString() );
        }
    }
}
