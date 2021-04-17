using System;
using System.Linq;
using Shouldly;
using Xunit;
namespace Mkb.EF.SqlRepository.Tests.IntegrationTests.AddMany
{
    public class AddManyTestsIntegrationSync : BaseIntegrationSyncTests
    {
        [Fact]
        public void Ensure_addMany_works()
        {
            var users = Enumerable.Range(1, 10)
                .Select(f => Guid.NewGuid().ToString("N"))
                .Select(f => new User {Email = $"{f}@gmail.com", Name = f})
                .ToArray();

            var testGuid = Guid.NewGuid().ToString("N");
            var email = $"{testGuid}@test.com";
            Sut.AddMany(users);
            Sut.Save();

            SimpleDbContext.Users.Count(f => users.Select(f => f.Name).Contains(f.Name)).ShouldBe(users.Count());
        }
    }
}