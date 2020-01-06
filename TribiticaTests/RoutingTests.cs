using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Threading.Tasks;
using Xunit;
using TribiticaMVP.Contracts;

namespace TribiticaTests
{
    public class RoutingTests : IClassFixture<WebApplicationFactory<TribiticaMVP.Startup>>
    {
        private readonly WebApplicationFactory<TribiticaMVP.Startup> _factory;

        public RoutingTests(WebApplicationFactory<TribiticaMVP.Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Account/Login")]
        public async Task GetTest_InputNonLoginRequiringEndpoints_ReturnsSuccessAndCorrectContentType(string url)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("/Kanban")]
        [InlineData("/Kanban/Index")]
        public async Task GetTest_InputTryLoginOnlyPagesWithoutLogin_ReturnsLoginPage(string url)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            Assert.Contains("/Account/Login", response.RequestMessage.RequestUri.ToString());
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
    }
}
