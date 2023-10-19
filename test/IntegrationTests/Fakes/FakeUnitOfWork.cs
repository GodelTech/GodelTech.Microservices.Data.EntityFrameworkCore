using GodelTech.Data.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.IntegrationTests.Fakes
{
    public class FakeUnitOfWork : UnitOfWork<DbContext>
    {
        public FakeUnitOfWork(IDbContextFactory<DbContext> dbContextFactory)
            : base(dbContextFactory)
        {

        }
    }
}
