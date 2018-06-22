using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using PlanningPokerBackend.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
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
        public async Task DataSeeder_Created_PlayTable()
        {
            var response = await _client.GetAsync("/api/playtables/getall");
            response.EnsureSuccessStatusCode();
            var tables = JsonConvert.DeserializeObject<List<PlayTable>>(await response.Content.ReadAsStringAsync());
            Assert.NotEmpty(tables);
        }
    }
}
