using System.Net.Http;

namespace PracticeWork10M4.Services;

public class HttpClientHandlerInsecure : HttpClientHandler
{
    public HttpClientHandlerInsecure()
    {
        ServerCertificateCustomValidationCallback =
            (sender, cert, chain, sslPolicyErrors) => true;
    }
}