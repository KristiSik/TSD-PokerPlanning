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
        public async Task Add_User_Action_Works_Good()
        {
            AddUser user = new AddUser() { Email = "somemail@mail.com", FirstName = "MyName", LastName = "MyLastName", Password = "12345" };

            var response = await _client.PostAsync("/api/users/add", new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            var list = JsonConvert.DeserializeObject<List<User>>((await _client.GetAsync("/api/users/getall").Result.Content.ReadAsStringAsync()));

            Assert.Single(list);
        }
        [Fact]
        public async Task Add_User_Action_Works_Good_2()
        {
            AddUser user = new AddUser() { Email = "somemail@mail.com", FirstName = "MyName", LastName = "MyLastName", Password = "12345" };

            var response = await _client.PostAsync("/api/users/add", new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            var list = JsonConvert.DeserializeObject<List<User>>((await _client.GetAsync("/api/users/getall").Result.Content.ReadAsStringAsync()));

            Assert.Single(list);
        }
    }
}
