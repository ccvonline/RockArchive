using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class ApnHttp2Sender : IDisposable
{
    private static readonly Dictionary<ApnServerType, string> servers = new Dictionary<ApnServerType, string>
        {
            {ApnServerType.Development, "https://api.development.push.apple.com:443" },
            {ApnServerType.Production, "https://api.push.apple.com:443" }
        };

    private const string apnidHeader = "apns-id";

    private readonly string p8privateKey;
    private readonly string p8privateKeyId;
    private readonly string teamId;
    private readonly string appBundleIdentifier;
    private readonly ApnServerType server;
    private readonly Lazy<string> jwtToken;
    private readonly Lazy<HttpClient> http;
    private readonly Lazy<Http2Handler> handler;

    /// <summary>
    /// Initialize sender
    /// </summary>
    /// <param name="p8privateKey">p8 certificate string</param>
    /// <param name="privateKeyId">10 digit p8 certificate id. Usually a part of a downloadable certificate filename</param>
    /// <param name="teamId">Apple 10 digit team id</param>
    /// <param name="appBundleIdentifier">App slug / bundle name</param>
    /// <param name="server">Development or Production server</param>
    public ApnHttp2Sender( string p8privateKey, string p8privateKeyId, string teamId, string appBundleIdentifier, ApnServerType server )
    {
        this.p8privateKey = p8privateKey;
        this.p8privateKeyId = p8privateKeyId;
        this.teamId = teamId;
        this.server = server;
        this.appBundleIdentifier = appBundleIdentifier;
        this.jwtToken = new Lazy<string>( () => CreateJwtToken() );
        this.handler = new Lazy<Http2Handler>( () => new Http2Handler() );
        this.http = new Lazy<HttpClient>( () => new HttpClient( handler.Value ) );
    }

    public async Task<ApnSendResult> SendAsync(
        string deviceToken,
        object notification,
        string apnsId = null,
        string apnsExpiration = "0",
        string apnsPriority = "10" )
    {
        var path = $"/3/device/{deviceToken}";
        var json = JsonConvert.SerializeObject( notification );
        var result = new ApnSendResult { NotificationId = apnsId };

        HttpResponseMessage response = null;

        try
        {
        Retry:
            var request = new HttpRequestMessage( HttpMethod.Post, new Uri( servers[server] + path ) )
            {
                Version = new Version( 2, 0 ),
                Content = new StringContent( json )
            };
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue( "bearer", jwtToken.Value );
            request.Headers.TryAddWithoutValidation( ":method", "POST" );
            request.Headers.TryAddWithoutValidation( ":path", path );
            request.Headers.Add( "apns-topic", appBundleIdentifier );
            request.Headers.Add( "apns-expiration", apnsExpiration );
            request.Headers.Add( "apns-priority", apnsPriority );
            if ( !string.IsNullOrWhiteSpace( apnsId ) )
            {
                request.Headers.Add( apnidHeader, apnsId );
            }

            response = await http.Value.SendAsync( request );

            if ( ( int ) response.StatusCode == 429/*HttpStatusCode.TooManyRequests*/ )
            {
                Console.WriteLine( "Retrying in a second" );
                await Task.Delay( 1000 );
                goto Retry;
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
            Console.WriteLine( ex );
            result.Exception = ex;
        }
        finally
        {
            if ( response != null )
            {
                result.Success = response.IsSuccessStatusCode;
                result.Status = response.StatusCode;
                result.NotificationId = response.Headers.GetValues( apnidHeader ).FirstOrDefault();
            }
        }

        return result;
    }

    public void Dispose()
    {
        if ( http.IsValueCreated )
        {
            handler.Value.Dispose();
            http.Value.Dispose();
        }
    }

    private string CreateJwtToken()
    {
        var header = JsonConvert.SerializeObject( new { alg = "ES256", kid = p8privateKeyId } );
        var payload = JsonConvert.SerializeObject( new { iss = teamId, iat = ToEpoch( DateTime.UtcNow ) } );

        var key = CngKey.Import( Convert.FromBase64String( p8privateKey ), CngKeyBlobFormat.Pkcs8PrivateBlob );
        using ( var dsa = new ECDsaCng( key ) )
        {
            dsa.HashAlgorithm = CngAlgorithm.Sha256;
            var headerBase64 = Convert.ToBase64String( Encoding.UTF8.GetBytes( header ) );
            var payloadBasae64 = Convert.ToBase64String( Encoding.UTF8.GetBytes( payload ) );
            var unsignedJwtData = $"{headerBase64}.{payloadBasae64}";
            var signature = dsa.SignData( Encoding.UTF8.GetBytes( unsignedJwtData ) );
            return $"{unsignedJwtData}.{Convert.ToBase64String( signature )}";
        }
    }

    private static int ToEpoch( DateTime time )
    {
        var span = time - new DateTime( 1970, 1, 1 );
        return Convert.ToInt32( span.TotalSeconds );
    }

    private class Http2Handler : WinHttpHandler { }
}

public class ApnSendResult
{
    public bool Success { get; set; }
    public HttpStatusCode Status { get; set; }
    public string NotificationId { get; set; }
    public Exception Exception { get; set; }

    public override string ToString()
    {
        return $"{Success}, {Status}, {Exception?.Message}";
    }
}

public enum ApnServerType
{
    Development,
    Production
}