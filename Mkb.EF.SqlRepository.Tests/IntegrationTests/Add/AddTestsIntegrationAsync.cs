using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;
[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace Mkb.EF.SqlRepository.Tests.IntegrationTests.Add
{
    public class AddTestsIntegrationAsync : BaseIntegrationAsyncTests
    {
        [Fact]
        public async Task Ensure_add_works()
        {
            var testGuid = Guid.NewGuid().ToString("N");
            var email = $"{testGuid}@test.com";
            Sut.Add(new User {Name = testGuid, Email = email});
            await Sut.Save();

            var item = await SimpleDbContext.Users.FirstAsync(f => f.Email == email);
            item.Name.ShouldBe(testGuid);
        }
    }
}