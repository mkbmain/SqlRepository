using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;
namespace Mkb.EF.SqlRepository.Tests.IntegrationTests.Delete
{
    public class DeleteIntegrationAsync : BaseIntegrationAsyncTests
    {
        [Fact]
        public async Task Ensure_we_can_delete()
        {
            var name = Guid.NewGuid().ToString("N");
            var email = $"{name}@test.com";
            var item = new User {Email = email, Name = name};
            SimpleDbContext.Users.Add(item);
            await SimpleDbContext.SaveChangesAsync();
            SimpleDbContext.Users.Any(t => t.Email == email).ShouldBe(true);

            Sut.Delete(item);
            await Sut.Save();

            SimpleDbContext.Users.Any(t => t.Email == email).ShouldBe(false);
        }
    }
}