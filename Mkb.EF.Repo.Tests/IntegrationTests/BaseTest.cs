using System;
using Mkb.EF.Repo;

namespace Mkb.EF.SqlRepository.Tests.IntegrationTests
{
    public abstract class BaseIntegrationTests<T> where T : SqlRepoBase
    {
        protected SimpleDbContext SimpleDbContext = new SimpleDbContext();
        protected T Sut { get; set; }
        public BaseIntegrationTests()
        {
            Sut = (T) Activator.CreateInstance(typeof(T), SimpleDbContext);
        }


    }
    
    public abstract class BaseIntegrationAsyncTests : BaseIntegrationTests<SqlRepositoryAsync<SimpleDbContext>>
    {
    }
    
    public abstract class BaseIntegrationSyncTests : BaseIntegrationTests<Repo.SqlRepository<SimpleDbContext>>
    {
    }
}