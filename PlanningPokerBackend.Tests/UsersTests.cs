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
    }
}
