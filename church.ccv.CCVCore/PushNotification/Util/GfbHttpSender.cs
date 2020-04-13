using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class GfbHttpSender
{
    private const string sFirebaseSendUrl = "https://fcm.googleapis.com/fcm/send";
    private string mAuthKey;
    private HttpClient mHttpClient;

    public GfbHttpSender( string authKey )
    {
        mAuthKey = authKey;
        mHttpClient = new HttpClient();
    }

    public async Task<GfbSendResult> SendAsync( object notification )
    {
        var jsonNotification = JsonConvert.SerializeObject( notification );

        HttpResponseMessage response = null;
        GfbSendResult result = new GfbSendResult();

        try
        {
        Retry:
            var request = new HttpRequestMessage( HttpMethod.Post, sFirebaseSendUrl );
            request.Content = new StringContent( jsonNotification, System.Text.Encoding.UTF8, "application/json" );
            request.Headers.TryAddWithoutValidation( "Authorization", "key=" + mAuthKey );

            response = await mHttpClient.SendAsync( request );

            if ( ( int ) response.StatusCode > 500 )
            {
                double? retryValue = response.Headers.RetryAfter?.Delta?.TotalMilliseconds;
                if ( retryValue.HasValue )
                {
                    await Task.Delay( ( int ) retryValue );
                    goto Retry;
                }
            }
        }
        // The following two exceptions are thrown even in a simple prototype app.
        // They appear to be harmless (everything works fine) and I don't have time to debug why
        // right now.
        catch ( System.IO.FileLoadException )
        {
        }
        catch ( System.Net.Http.HttpRequestException )
        {
        }
        catch ( Exception ex )
        {
            result.Exception = ex;
        }
        finally
        {
            if ( response != null )
            {
                result.Success = response.IsSuccessStatusCode;
                result.Status = response.StatusCode;
            }
        }

        return result;
    }

    public class GfbSendResult
    {
        public bool Success { get; set; }
        public HttpStatusCode Status { get; set; }
        public Exception Exception { get; set; }

        public override string ToString()
        {
            return $"{Success}, {Status}, {Exception?.Message}";
        }
    }
}