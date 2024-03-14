using RestSharp;
using RestSharp.Authenticators;

class Program
{
    static void Main(string[] args)
    {
        var client = new RestClient(new RestClientOptions("https://api.github.com")
        {
            
                Authenticator = new HttpBasicAuthenticator("StanM84", "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")
           
        });
        var request = new RestRequest("/repos/testnakov/test-nakov-repo/issues", Method.Post);

        request.AddHeader("Content-Type", "application/json");
        request.AddJsonBody(new { title = "TestinIssue3/6", body = "some testing body" });

        var response = client.Execute(request);
    }
}