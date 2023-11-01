using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Data;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Data.Entities;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Models.Bank;
using Xunit;
using Xunit.Abstractions;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.IntegrationTests
{
    public sealed class DataInitializerTests : IDisposable
    {
        private readonly AppTestFixture _fixture;
        private readonly CurrencyExchangeRateDbContext _dbContext;
        private readonly HttpClient _client;

        public DataInitializerTests(ITestOutputHelper output)
        {
            _fixture = new AppTestFixture
            {
                Output = output
            };

            _dbContext = _fixture.GetDbContext();

            Seed();

            _client = _fixture.CreateClient();
        }

        public void Dispose()
        {
            _client.Dispose();
            _dbContext.Dispose();
            _fixture.Dispose();
        }

        private void Seed()
        {
            _dbContext.Set<BankEntity>().AddRange(
                new BankEntity
                {
                    Name = "First Bank Name"
                },
                new BankEntity
                {
                    Name = "Second Bank Name"
                }
            );

            _dbContext.SaveChanges();
        }

        [Fact]
        public async Task Configure_Success()
        {
            // Arrange
            var expectedResult = _dbContext.Set<BankEntity>().ToList();

            // Act
            var result = await _client.GetAsync(new Uri("/banks", UriKind.Relative));

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            var resultValue = await result.Content.ReadFromJsonAsync<IList<BankModel>>();
            Assert.Equal(2, resultValue.Count);
            for (var i = 0; i < 2; i++)
            {
                Assert.Equal(expectedResult[i].Id, resultValue[i].Id);
                Assert.Equal(expectedResult[i].Name, resultValue[i].Name);
            }
        }

        [Fact]
        public async Task WithRepository_Success()
        {
            // Arrange & Act
            var result = await _client.GetAsync(new Uri("/currencies/count", UriKind.Relative));

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            var resultValue = await result.Content.ReadFromJsonAsync<int>();
            Assert.Equal(0, resultValue);
        }
    }
}
