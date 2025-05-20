using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace GatewayApi.Tests.Controllers
{
    public class GatewayIntegrationTests
        : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public GatewayIntegrationTests(WebApplicationFactory<Program> factory)
            => _factory = factory;

        [Fact]
        public async Task Get_NonExistingFile_ShouldReturn_Unauthorized()
        {
    
            var client = _factory.CreateClient();
            var fakeId = "00000000-0000-0000-0000-000000000000";

          
            var response = await client.GetAsync($"/files/storage/{fakeId}");


            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

    }
}