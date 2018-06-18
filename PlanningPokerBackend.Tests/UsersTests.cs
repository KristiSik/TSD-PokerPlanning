using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using PlanningPokerBackend.Models;
using PlanningPokerBackend.Models.PostRequestBodyModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PlanningPokerBackend.Tests
{
    public class UsersTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        public UsersTests()
        {
            // Arrange
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            _client = _server.CreateClient();
        }
        [Fact]
        public async Task Users_Table_Exists()
        {
            var response = await _client.GetAsync("/api/users/getall");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task DataSeeder_Created_Users()
        {
            var response = await _client.GetAsync("/api/users/getall");
            response.EnsureSuccessStatusCode();
            var users = JsonConvert.DeserializeObject<List<User>>(await response.Content.ReadAsStringAsync());
            Assert.NotEmpty(users);
        }
        [Fact]
        public async Task Set_IsReady_Status_Works()
        {
            LoginUser dave = new LoginUser() { Email = "davemurray@mail.com", Password = "password" };
            var response = await _client.PostAsync("/api/users/login", new StringContent(JsonConvert.SerializeObject(dave), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            string token = await response.Content.ReadAsStringAsync();
            var response2 = await _client.PostAsync("/api/users/setreadystatus", new StringContent(JsonConvert.SerializeObject(new TokenAndIsReadyStatusBody() { UserToken = token, IsReady = true }), Encoding.UTF8, "application/json"));
            response2.EnsureSuccessStatusCode();
            var response3 = await _client.GetAsync("/api/users/getall");
            response3.EnsureSuccessStatusCode();
            string result = await response3.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<User>>(result);
            bool daveIsReady = users.FirstOrDefault(u => u.Email == dave.Email).IsReady;
            Assert.True(daveIsReady);
        }
    }
}
