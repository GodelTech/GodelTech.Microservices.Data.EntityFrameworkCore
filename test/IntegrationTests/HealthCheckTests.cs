using System;
using System.Net;
using System.Threading.Tasks;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.IntegrationTests
{
    public class HealthCheckTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _fixture;

        public HealthCheckTests(WebApplicationFactory<Startup> fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Success()
        {
            // Arrange
            var client = _fixture.CreateClient();

            // Act
            var response = await client.GetAsync(new Uri("/health", UriKind.Relative));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
