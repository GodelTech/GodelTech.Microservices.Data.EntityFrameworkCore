using Xunit;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Tests
{
    public class DataInitializerOptionsTests
    {
        private readonly DataInitializerOptions _options;

        public DataInitializerOptionsTests()
        {
            _options = new DataInitializerOptions();
        }

        [Fact]
        public void ConnectionStringName_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal("DefaultConnection", _options.ConnectionStringName);
        }

        [Fact]
        public void ConnectionStringName_Set_Success()
        {
            // Arrange
            var expectedResult = "Test ConnectionStringName";

            // Act
            _options.ConnectionStringName = expectedResult;

            // Assert
            Assert.Equal(expectedResult, _options.ConnectionStringName);
        }

        [Fact]
        public void SqlServerHealthCheckName_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal("sqlserver", _options.SqlServerHealthCheckName);
        }

        [Fact]
        public void SqlServerHealthCheckName_Set_Success()
        {
            // Arrange
            var expectedResult = "Test SqlServerHealthCheckName";

            // Act
            _options.SqlServerHealthCheckName = expectedResult;

            // Assert
            Assert.Equal(expectedResult, _options.SqlServerHealthCheckName);
        }

        [Fact]
        public void EnableDatabaseMigration_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.False(_options.EnableDatabaseMigration);
        }

        [Fact]
        public void EnableDatabaseMigration_Set_Success()
        {
            // Arrange && Act
            _options.EnableDatabaseMigration = true;

            // Assert
            Assert.True(_options.EnableDatabaseMigration);
        }
    }
}
