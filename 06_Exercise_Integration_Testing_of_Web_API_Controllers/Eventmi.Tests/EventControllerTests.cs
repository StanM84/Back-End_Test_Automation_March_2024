using Eventmi.Core.Models.Event;
using RestSharp;
using System.Net;

namespace Eventmi.Tests
{
    public class Tests
    {
        private RestClient _client;
        private string _baseUrl = "https://localhost:7236";
        [SetUp]
        public void Setup()
        {
            _client = new RestClient(_baseUrl);
        }

        [Test]
        public async Task GetAllEvents_ReturnsSuccessStatusCode()
        {
            // Arrange
            var request = new RestRequest("/Event/All", Method.Get);

            // Act
            var response = await _client.ExecuteAsync(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Add_GetRequest_ReturnsAddView()
        {
            // Arrange
            var request = new RestRequest("/Event/Add", Method.Get);

            // Act
            var responce = await _client.ExecuteAsync(request);

            // Assert
            Assert.That(responce.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Add_PostRequest_AddNewEventAddRedirects()
        {
            // Arrange
            var input = new EventFormModel()
            {
                Name = "M_Markov",
                Start = new DateTime(2024, 08, 08, 12, 0, 0),
                End = new DateTime(2024, 09, 09, 13, 0, 0),
                Place = "Ruse"
            };

            var request = new RestRequest("/Event/Add", Method.Post);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            request.AddParameter("Name", input.Name);
            request.AddParameter("Start", input.Start.ToString("MM/dd/yyyy hh:mm tt"));
            request.AddParameter("End", input.End.ToString("MM/dd/yyyy hh:mm tt")); 
            request.AddParameter("Place", input.Place);

            // Act
            var responce = await _client.ExecuteAsync(request);

            // Assert
            Assert.That(responce.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}