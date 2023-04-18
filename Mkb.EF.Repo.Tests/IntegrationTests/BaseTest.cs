using System;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Mkb.EF.Repo;

namespace Mkb.EF.SqlRepository.Tests.IntegrationTests
{
    public abstract class BaseIntegrationTests<T> where T : SqlRepoBase
    {
        private static bool Drop = true;
        protected SimpleDbContext SimpleDbContext = new SimpleDbContext();
        protected T Sut { get; set; }

        public BaseIntegrationTests()
        {
            Sut = (T)Activator.CreateInstance(typeof(T), SimpleDbContext);
            if (Drop)
            {
                Drop = false;
                using var sql = new SqlConnection(Tests.SimpleDbContext.MasterString);
                try
                {
                    sql.Open();
                    var command = new SqlCommand($"drop database IF EXISTS " + Tests.SimpleDbContext.DbName, sql);
                    command.ExecuteNonQuery();
                    sql.Close();
                }
                catch (Exception e)
                {
                }
            }

            SimpleDbContext.Database.Migrate();
        }
    }

    public abstract class BaseIntegrationAsyncTests : BaseIntegrationTests<SqlRepositoryAsync<SimpleDbContext>>
    {
    }

    public abstract class BaseIntegrationSyncTests : BaseIntegrationTests<Repo.SqlRepository<SimpleDbContext>>
    {
    }
}