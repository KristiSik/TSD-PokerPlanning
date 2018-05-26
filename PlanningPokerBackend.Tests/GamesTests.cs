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
    public class GamesTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        public GamesTests()
        {
            // Arrange
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            _client = _server.CreateClient();
        }
        [Fact]
        public async Task Game_Successfuly_Starts()
        {
            LoginUser dave = new LoginUser() { Email = "davemurray@mail.com", Password = "password" };
            var response = await _client.PostAsync("/api/users/login", new StringContent(JsonConvert.SerializeObject(dave), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            string token = await response.Content.ReadAsStringAsync();
            Assert.NotNull(token);
            Assert.NotEmpty(token);
            var response2 = await _client.PostAsync("/api/games/start", new StringContent(JsonConvert.SerializeObject(new TokenBody() { Token = token }), Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
        }
    }
}
