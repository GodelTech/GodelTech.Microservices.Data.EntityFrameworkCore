using System;
using GodelTech.Microservices.Data.EntityFrameworkCore.Tests.Fakes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Moq;
using Xunit;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Tests
{
    public class DataInitializerTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IHostEnvironment> _mockHostEnvironment;

        public DataInitializerTests()
        {
            _mockConfiguration = new Mock<IConfiguration>(MockBehavior.Strict);
            _mockHostEnvironment = new Mock<IHostEnvironment>(MockBehavior.Strict);
        }

        [Fact]
        public void ConfigureSqlServerDbContextOptionsBuilder_WhenOptionsIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var initializer = new FakeDataInitializer(_mockConfiguration.Object, _mockHostEnvironment.Object);

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(
                () => initializer.ExposedConfigureSqlServerDbContextOptionsBuilder(null)
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
            var sqlServerOptionsAction = new Action<SqlServerDbContextOptionsBuilder>(_ => { sqlServerOptionsActionInvoked = true; });

            var mockSqlServerOptionsAction = new Mock<Action<SqlServerDbContextOptionsBuilder>>(MockBehavior.Strict);
            mockSqlServerOptionsAction
                .Setup(x => x.ToString())
                .Returns(string.Empty);

            var initializer = new FakeDataInitializer(
                _mockConfiguration.Object,
                _mockHostEnvironment.Object,
                sqlServerOptionsAction: sqlServerOptionsAction
            );

            // Act
            initializer.ExposedConfigureSqlServerDbContextOptionsBuilder(mockSqlServerDbContextOptionsBuilder.Object);

            // Assert
            Assert.True(sqlServerOptionsActionInvoked);

            mockSqlServerDbContextOptionsBuilder
                .Verify(x => x.EnableRetryOnFailure(), Times.Once);
        }

        [Fact]
        public void MigrateDatabase_WhenApplicationBuilderIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var initializer = new FakeDataInitializer(_mockConfiguration.Object, _mockHostEnvironment.Object);

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(
                () => initializer.ExposedMigrateDatabase(null)
            );
            Assert.Equal("app", exception.ParamName);
        }
    }
}
