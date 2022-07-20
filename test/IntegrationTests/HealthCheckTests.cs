using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.IntegrationTests
{
    public sealed class HealthCheckTests : IDisposable
    {
        private readonly AppTestFixture _fixture;

        public HealthCheckTests(ITestOutputHelper output)
        {
            _fixture = new AppTestFixture
            {
                Output = output
            };
        }

        public void Dispose()
        {
            _fixture.Dispose();
        }

        [Fact]
        public async Task Success()
        {
            // Arrange
            var client = _fixture.CreateClient();

            // Act
            var response = await client.GetAsync(new Uri("/health", UriKind.Relative));

            // Assert
            Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
        }
    }
}
