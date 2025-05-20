using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FileAnalysisService.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace AnalysisApi.Tests.Controllers
{
    public class AnalysisControllerTests
        : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AnalysisControllerTests(WebApplicationFactory<Program> factory)
            => _client = factory.CreateClient();

        [Fact]
        public async Task Post_ValidContent_ReturnsStatistics()
        {
            
            var dto = new FileContentDto("One two three");
            var content = new StringContent(
                JsonConvert.SerializeObject(dto),
                Encoding.UTF8,
                "application/json"
            );
        
            var resp = await _client.PostAsync("/files/analysis", content);
           
            resp.StatusCode.Should().Be(HttpStatusCode.OK);
            var body = await resp.Content.ReadAsStringAsync();
            body.Should().Contain("\"Words\":3").And.Contain("\"Paragraphs\":1");
        }
    }
}