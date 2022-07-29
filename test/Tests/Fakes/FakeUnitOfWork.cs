using GodelTech.Data.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GodelTech.Microservices.Data.EntityFrameworkCore.Tests.Fakes
{
    public class FakeUnitOfWork : UnitOfWork<DbContext>
    {
        public FakeUnitOfWork(IDbContextFactory<DbContext> dbContextFactory)
            : base(dbContextFactory)
        {

        }
    }
}
