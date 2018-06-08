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
                .UseStartup<Startup>().ConfigureServices((IServiceCollection services) => {
                    services.AddDbContext<PlanningPokerDbContext>(opt => opt.UseInMemoryDatabase("somedb"));
                    services.AddMvc()
                        .AddJsonOptions(options => {
                            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                        });
                }));
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
        [Fact]
        public async Task IsStarted_Returns_True_If_Game_Is_Started()
        {
            LoginUser dave = new LoginUser() { Email = "davemurray@mail.com", Password = "password" };
            var response = await _client.PostAsync("/api/users/login", new StringContent(JsonConvert.SerializeObject(dave), Encoding.UTF8, "application/json"));
            string token = await response.Content.ReadAsStringAsync();
            var response2 = await _client.PostAsync("/api/games/start", new StringContent(JsonConvert.SerializeObject(new TokenBody() { Token = token }), Encoding.UTF8, "application/json"));
            var response3 = await _client.PostAsync("/api/games/isstarted", new StringContent(JsonConvert.SerializeObject(new TokenBody() { Token = token }), Encoding.UTF8, "application/json"));
            response3.EnsureSuccessStatusCode();
            string result = await response3.Content.ReadAsStringAsync();
            Assert.Equal("true", result);
        }
    }
}
