using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using PlanningPokerBackend.Models;
using PlanningPokerBackend.Models.PostRequestBodyModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PlanningPokerBackend.Tests
{
    public class PlayTableTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        public PlayTableTests()
        {
            // Arrange
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            _client = _server.CreateClient();
        }
        [Fact]
        public async Task PlayTables_Table_Exists()
        {
            var response = await _client.GetAsync("/api/playtables/getall");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task GetParticipants_Returns_Bad_Request_If_Wrong_Token()
        {
            var response = await _client.GetAsync("/api/playtables/getparticipants?token=1");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
