using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.IntegrationTests
{
    public sealed class SimpleHealthCheckTests : IDisposable
    {
        private readonly SimpleAppTestFixture _fixture;

        public SimpleHealthCheckTests(ITestOutputHelper output)
        {
            _fixture = new SimpleAppTestFixture
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
