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
            var response2 = await _client.PostAsync("/api/games/start", new StringContent(JsonConvert.SerializeObject(new TokenBody() { Token = DataSeeder.Users.Find(u => u.FirstName == "Dave").Token }), Encoding.UTF8, "application/json"));
            string result = await response2.Content.ReadAsStringAsync();
            Assert.Equal("Not everyone is ready", result);
        }
        [Fact]
        public async Task Only_Admin_Of_Table_Can_Start_New_Game()
        {
            var response2 = await _client.PostAsync("/api/games/start", new StringContent(JsonConvert.SerializeObject(new TokenBody() { Token = DataSeeder.Users.Find(u => u.FirstName == "Steve").Token }), Encoding.UTF8, "application/json"));
            string result = await response2.Content.ReadAsStringAsync();
            Assert.Equal("Only admin can start new game", result);
        }
        [Fact]
        public async Task Game_Starts_Successfuly_If_All_Participants_Are_Ready()
        {
            foreach(var user in DataSeeder.Users)
            {
                var response = await _client.PostAsync("/api/users/setreadystatus", new StringContent(JsonConvert.SerializeObject(new TokenAndIsReadyStatusBody() { UserToken = user.Token, IsReady = true }), Encoding.UTF8, "application/json"));
            }
            var response2 = await _client.PostAsync("/api/games/start", new StringContent(JsonConvert.SerializeObject(new TokenBody() { Token = DataSeeder.Users.Find(u => u.FirstName == "Dave").Token }), Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
        }
        [Fact]
        public async Task IsStarted_Returns_True_If_Game_Is_Started()
        {
            foreach (var user in DataSeeder.Users)
            {
                var response = await _client.PostAsync("/api/users/setreadystatus", new StringContent(JsonConvert.SerializeObject(new TokenAndIsReadyStatusBody() { UserToken = user.Token, IsReady = true }), Encoding.UTF8, "application/json"));
            }
            var response2 = await _client.PostAsync("/api/games/start", new StringContent(JsonConvert.SerializeObject(new TokenBody() { Token = DataSeeder.Users.Find(u => u.FirstName == "Dave").Token }), Encoding.UTF8, "application/json"));
            var response3 = await _client.GetAsync(string.Format("/api/games/isstarted?token={0}", DataSeeder.Users.Find(u => u.FirstName == "Steve").Token));
            var result = await response3.Content.ReadAsStringAsync();
            Assert.Equal("true", result);
        }
        [Fact]
        public async Task IsFinished_Returns_True_When_All_Participants_Are_With_Their_Answers()
        {
            foreach (var user in DataSeeder.Users)
            {
                var response = await _client.PostAsync("/api/users/setreadystatus", new StringContent(JsonConvert.SerializeObject(new TokenAndIsReadyStatusBody() { UserToken = user.Token, IsReady = true }), Encoding.UTF8, "application/json"));
            }
            var response2 = await _client.PostAsync("/api/games/start", new StringContent(JsonConvert.SerializeObject(new TokenBody() { Token = DataSeeder.Users.Find(u => u.FirstName == "Dave").Token }), Encoding.UTF8, "application/json"));
            foreach (var user in DataSeeder.Users)
            {
                var response = await _client.PostAsync("/api/games/setreadystatus", new StringContent(JsonConvert.SerializeObject(new TokenAndIsReadyStatusBody() { UserToken = user.Token, IsReady = true }), Encoding.UTF8, "application/json"));
            }
            var response3 = await _client.GetAsync(string.Format("/api/games/isfinished?token={0}", DataSeeder.Users.Find(u => u.FirstName == "Steve").Token));
            var result = await response3.Content.ReadAsStringAsync();
            Assert.Equal("true", result);
        }
    }
}
