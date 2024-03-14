using RestSharp.Authenticators;
using RestSharp;
using System.Net;
using Newtonsoft.Json;

namespace RestSharpTestProject
{
    public class Tests
    {
        RestClient client;
        [SetUp]
        public void Setup()
        {
            var options = new RestClientOptions("https://api.github.com")
            {
                MaxTimeout = 3000,
                Authenticator = new HttpBasicAuthenticator("StanM84", "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")
            };
            client = new RestClient(options);
        }

        [Test]
        public void Test_GitGetIssuesEndpoint()
        {
            // Arrange
            //var client = new RestClient("https://api.github.com");
            var request = new RestRequest("/repos/testnakov/test-nakov-repo/issues", Method.Get);

            // Act
            var response = client.Execute(request);

            // Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }


        [Test]
        public void Test_GitGetIssuesEndpoint_MoreValidation()
        {
            // Arrange
            //var client = new RestClient("https://api.github.com");
            var request = new RestRequest("/repos/testnakov/test-nakov-repo/issues", Method.Get);

            // Act
            var response = client.Execute(request);
            var issueObjects = JsonConvert.DeserializeObject<List<issue>>(response.Content);

            // Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public void Test_GitPostMetod()
        {
            // Arrange & Act
            var createdIssue = CreateIssue("IssueTest123", "BodyTest123");

            // Assert
            Assert.That(createdIssue.title.Equals("IssueTest123"));
            Assert.That(createdIssue.body.Equals("BodyTest123"));

        }

        private issue CreateIssue (string title, string body)
        {
            var request = new RestRequest("/repos/testnakov/test-nakov-repo/issues", Method.Post);
            request.AddBody(new { body, title } );

            // Act
            var response = client.Execute(request);
            var issueObject = JsonConvert.DeserializeObject<issue>(response.Content);

            return issueObject;
        }
    }
}