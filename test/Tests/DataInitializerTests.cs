using System;
using System.Collections.Generic;
using GodelTech.Microservices.Data.EntityFrameworkCore.Tests.Fakes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Moq;
using Xunit;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Tests
{
    public class DataInitializerTests
    {
        private readonly FakeDataInitializer _initializer;

        public DataInitializerTests()
        {
            var inMemorySettings = new Dictionary<string, string> {
                {
                    "ConnectionStrings:DefaultConnection",
                    "TestConnectionString"
                }
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var mockHostEnvironment = new Mock<IHostEnvironment>(MockBehavior.Strict);

            _initializer = new FakeDataInitializer(
                configuration,
                mockHostEnvironment.Object
            );
        }

        [Fact]
        public void MigrateDatabase_WhenAppIsNull_ThrowsArgumentNullException()
        {
            // Arrange & Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(
                () => _initializer.ExposedMigrateDatabase(null)
            );
            Assert.Equal("app", exception.ParamName);
        }
    }
}
