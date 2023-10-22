﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Data;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Data.Entities;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Models.Bank;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.IntegrationTests
{
    public sealed class SimpleDataInitializerTests : IDisposable
    {
        private readonly SimpleAppTestFixture _fixture;

        public SimpleDataInitializerTests(ITestOutputHelper output)
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

        private static void Seed(CurrencyExchangeRateDbContext dbContext)
        {
            dbContext.Set<BankEntity>().AddRange(
                new BankEntity
                {
                    Name = "First Bank Name"
                },
                new BankEntity
                {
                    Name = "Second Bank Name"
                }
            );

            dbContext.SaveChanges();
        }

        [Fact]
        public async Task Configure_Success()
        {
            // Arrange
            var client = _fixture.CreateClient();

            using var scope = _fixture.Services.CreateScope();
            await using var dbContext = SimpleAppTestFixture.InitializeDbContextForTest(scope, Seed);

            var expectedResult = dbContext.Set<BankEntity>().ToList();

            // Act
            var result = await client.GetAsync(new Uri("/banks", UriKind.Relative));

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
            // Arrange
            var client = _fixture.CreateClient();

            // Act
            var result = await client.GetAsync(new Uri("/currencies/count", UriKind.Relative));

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            var resultValue = await result.Content.ReadFromJsonAsync<int>();
            Assert.Equal(0, resultValue);
        }
    }
}
