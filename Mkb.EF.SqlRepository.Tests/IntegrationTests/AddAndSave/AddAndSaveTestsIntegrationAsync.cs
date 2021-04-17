using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace Mkb.EF.SqlRepository.Tests.IntegrationTests.AddAndSave
{
    public class AddAndSaveTestsIntegrationAsync : BaseIntegrationAsyncTests
    {
        [Fact]
        public async Task Ensure_addAndSave_works()
        {
            var testGuid = Guid.NewGuid().ToString("N");
            var email = $"{testGuid}@test.com";
            await Sut.AddAndSave(new User {Name = testGuid, Email = email});

            var item = await SimpleDbContext.Users.FirstAsync(f => f.Email == email);
            item.Name.ShouldBe(testGuid);
        }
    }
}