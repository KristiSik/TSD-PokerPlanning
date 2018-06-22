using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
        public async Task Game_Doesnt_Start_If_Not_Everyone_Is_Ready()
        {
            LoginUser dave = new LoginUser() { Email = "davemurray@mail.com", Password = "password" };
            var response = await _client.PostAsync("/api/users/login", new StringContent(JsonConvert.SerializeObject(dave), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            string token = await response.Content.ReadAsStringAsync();
            Assert.NotNull(token);
            Assert.NotEmpty(token);
            var response2 = await _client.PostAsync("/api/games/start", new StringContent(JsonConvert.SerializeObject(new TokenBody() { Token = token }), Encoding.UTF8, "application/json"));
            string result = await response2.Content.ReadAsStringAsync();
            Assert.Equal("Not everyone is ready", result);
        }
        [Fact]
        public async Task Only_Admin_Of_Table_Can_Start_New_Game()
        {
            LoginUser dave = new LoginUser() { Email = "steveharris@mail.com", Password = "password" };
            var response = await _client.PostAsync("/api/users/login", new StringContent(JsonConvert.SerializeObject(dave), Encoding.UTF8, "application/json"));
            string token = await response.Content.ReadAsStringAsync();
            var response2 = await _client.PostAsync("/api/games/start", new StringContent(JsonConvert.SerializeObject(new TokenBody() { Token = token }), Encoding.UTF8, "application/json"));
            string result = await response2.Content.ReadAsStringAsync();
            Assert.Equal("Only admin can start new game", result);
        }
    }
}
