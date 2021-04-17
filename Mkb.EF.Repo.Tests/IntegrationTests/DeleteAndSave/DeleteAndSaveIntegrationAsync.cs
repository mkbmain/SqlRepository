using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Mkb.EF.SqlRepository.Tests.IntegrationTests.DeleteAndSave
{
    public class DeleteAndSaveIntegrationAsync : BaseIntegrationAsyncTests
    {
        [Fact]
        public async Task Ensure_we_can_deleteAndSave()
        {
            var name = Guid.NewGuid().ToString("N");
            var email = $"{name}@test.com";
            var item = new User {Email = email, Name = name};
            SimpleDbContext.Users.Add(item);
            await SimpleDbContext.SaveChangesAsync();
            SimpleDbContext.Users.Any(t => t.Email == email).ShouldBe(true);

            await Sut.DeleteAndSave(item);


            SimpleDbContext.Users.Any(t => t.Email == email).ShouldBe(false);
        }
    }
}