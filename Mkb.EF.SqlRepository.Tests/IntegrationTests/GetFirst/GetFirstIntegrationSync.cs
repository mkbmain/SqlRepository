using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Mkb.EF.SqlRepository.Tests.IntegrationTests.GetFirst
{
    public class GetFirstIntegrationSync : BaseIntegrationSyncTests
    {
        [Fact]
        public async Task Ensure_we_can_GetFirst()
        {
            var name = Guid.NewGuid().ToString("N");
            var email = $"{name}@test.com";
            var item = new User {Email = email, Name = name};
            SimpleDbContext.Users.Add(item);
            await SimpleDbContext.SaveChangesAsync();
            SimpleDbContext.Users.Any(t => t.Email == email).ShouldBe(true);

            var item2 = Sut.GetFirst<User>(f => f.Email == email);

            item2.ShouldNotBeNull();
            item2.Email.ShouldBe(item.Email);
            item2.Name.ShouldBe(item.Name);
            item2.Id.ShouldBe(item.Id);
        }
    }
}