namespace church.ccv.CCVCore.PushNotification.Util
{
    public class PushNotificationConstants
    {
        /// <summary>
        /// Platforms supported for Push Notification.
        /// </summary>
        public static class Platform
        {
            public const string iOS = "iOS";
            public const string Android = "Android";
        }

        /// <summary>
        /// Defines the types of metadata supported in the push notification
        /// </summary>
        public static class MetaDataKeys
        {
            public const string DeepLink = "deep-link";

            /// <summary>
            /// The available deep links to send a user to when they interact with the notification
            /// </summary>
            public static class DeepLinks
            {
                public const string WatchPage = "ccv://home/watch";
            }
        }

        /// <summary>
        /// Defines the keys that should be used to create the settings matrix for Apple Push Notification values.
        /// </summary>
        public static class APNMatrixKeys
        {
            public const string APNMatrixKey = "ApplePushNotificationSettings";
            public const string P8PrivateKey = "P8PrivateKey";
            public const string P8PrivateKeyId = "P8PrivateKeyId";
            public const string TeamId = "TeamId";
            public const string AppBundleIdentifier = "AppBundleIdentifier";
            public const string ApnServerType = "ApnServerType";
        }

        /// <summary>
        /// Defines the name, class and method for using the AppleHttp2Sender via reflection
        /// </summary>
        public static class APNSenderAssembly
        {
            public const string Name = "church.ccv.APNSender";
            public const string ClassType = "ApnHttp2Sender";
            public const string SendAsyncMethod = "SendAsync";
            public const string DisposeMethod = "Dispose";
        }

        /// <summary>
        /// Defines the two types of Apple Push Notification Servers
        /// </summary>
        public enum ApnServerType
        {
            Development,
            Production
        }
    }
}
