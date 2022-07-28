using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Data.Entities;
using GodelTech.Microservices.Data.EntityFrameworkCore.Demo.Simple.Models.Bank;
using Xunit;
using Xunit.Abstractions;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.IntegrationTests.Simple
{
    public sealed class SimpleDataInitializerTests : IDisposable
    {
        private readonly AppTestFixture _fixture;

        public SimpleDataInitializerTests(ITestOutputHelper output)
        {
            _fixture = new AppTestFixture
            {
                Output = output
            };

            Seed();
        }

        public void Dispose()
        {
            _fixture.Dispose();
        }

        private void Seed()
        {
            _fixture.DbContext.Set<BankEntity>().AddRange(
                new BankEntity
                {
                    Name = "First Bank Name"
                },
                new BankEntity
                {
                    Name = "Second Bank Name"
                }
            );

            _fixture.DbContext.SaveChanges();
        }

        [Fact]
        public async Task Configure_Success()
        {
            // Arrange
            var expectedResult = _fixture.DbContext.Set<BankEntity>().ToList();

            var client = _fixture.CreateClient();

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
    }
}
