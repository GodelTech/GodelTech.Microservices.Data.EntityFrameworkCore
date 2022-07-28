using System;
using System.Collections.Generic;
using GodelTech.Microservices.Data.EntityFrameworkCore.Tests.Fakes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Moq;
using Xunit;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Tests
{
    public class DataInitializerBaseTests
    {
        private readonly IConfiguration _configuration;
        private readonly Mock<IHostEnvironment> _mockHostEnvironment;

        private readonly FakeDataInitializerBase _initializer;

        public DataInitializerBaseTests()
        {
            var inMemorySettings = new Dictionary<string, string> {
                {
                    "ConnectionStrings:DefaultConnection",
                    "TestConnectionString"
                }
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _mockHostEnvironment = new Mock<IHostEnvironment>(MockBehavior.Strict);

            _initializer = new FakeDataInitializerBase(
                _configuration,
                _mockHostEnvironment.Object
            );
        }

        [Fact]
        public void ConfigureServicesList_Get()
        {
            // Arrange & Act & Assert
            Assert.Empty(_initializer.ExposedConfigureServicesList);
        }

        [Fact]
        public void ConfigureDbContextOptionsBuilder_WhenOptionsIsNull_ThrowsArgumentNullException()
        {
            // Arrange & Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(
                () => _initializer.ExposedConfigureDbContextOptionsBuilder(null)
            );
            Assert.Equal("options", exception.ParamName);
        }

        [Theory]
        [InlineData("Development", true)]
        [InlineData("NotDevelopment", false)]
        public void ConfigureDbContextOptionsBuilder_Success(
            string environmentName,
            bool isDevelopment)
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder().Options;

            var mockDbContextOptionsBuilder = new Mock<DbContextOptionsBuilder>(MockBehavior.Strict);
            mockDbContextOptionsBuilder
                .Setup(x => x.Options)
                .Returns(dbContextOptions);

            _mockHostEnvironment
                .Setup(x => x.EnvironmentName)
                .Returns(environmentName);

            mockDbContextOptionsBuilder
                .Setup(x => x.EnableSensitiveDataLogging(isDevelopment))
                .Returns(mockDbContextOptionsBuilder.Object);

            // Act
            _initializer.ExposedConfigureDbContextOptionsBuilder(mockDbContextOptionsBuilder.Object);

            // Assert
            mockDbContextOptionsBuilder
                .Verify(
                    x => x.EnableSensitiveDataLogging(isDevelopment),
                    Times.Once
                );
        }

        [Fact]
        public void ConfigureSqlServerDbContextOptionsBuilder_WhenOptionsIsNull_ThrowsArgumentNullException()
        {
            // Arrange & Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(
                () => _initializer.ExposedConfigureSqlServerDbContextOptionsBuilder(null)
            );
            Assert.Equal("options", exception.ParamName);
        }

        [Fact]
        public void ConfigureSqlServerDbContextOptionsBuilder_Success()
        {
            // Arrange
            var mockDbContextOptionsBuilder = new Mock<DbContextOptionsBuilder>(MockBehavior.Strict);
            var mockSqlServerDbContextOptionsBuilder = new Mock<SqlServerDbContextOptionsBuilder>(MockBehavior.Strict, mockDbContextOptionsBuilder.Object);
            mockSqlServerDbContextOptionsBuilder
                .Setup(x => x.EnableRetryOnFailure())
                .Returns(mockSqlServerDbContextOptionsBuilder.Object);

            var sqlServerOptionsActionInvoked = false;

            var initializer = new FakeDataInitializerBase(
                _configuration,
                _mockHostEnvironment.Object,
                sqlServerOptionsAction: _ => { sqlServerOptionsActionInvoked = true; }
            );

            // Act
            initializer.ExposedConfigureSqlServerDbContextOptionsBuilder(mockSqlServerDbContextOptionsBuilder.Object);

            // Assert
            Assert.True(sqlServerOptionsActionInvoked);

            mockSqlServerDbContextOptionsBuilder
                .Verify(x => x.EnableRetryOnFailure(), Times.Once);
        }
    }
}
