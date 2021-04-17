using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace Mkb.EF.SqlRepository.Tests.IntegrationTests.AddMany
{
    public class AddManyTestsIntegrationAsync : BaseIntegrationAsyncTests
    {
        [Fact]
        public async Task Ensure_addMany_works()
        {
            var users = Enumerable.Range(1, 10)
                .Select(f => Guid.NewGuid().ToString("N"))
                .Select(f => new User {Email = $"{f}@gmail.com", Name = f})
                .ToArray();

            var testGuid = Guid.NewGuid().ToString("N");
            var email = $"{testGuid}@test.com";
            Sut.AddMany(users);
            await Sut.Save();

            SimpleDbContext.Users.Count(f => users.Select(f => f.Name).Contains(f.Name)).ShouldBe(users.Count());
        }
    }
}