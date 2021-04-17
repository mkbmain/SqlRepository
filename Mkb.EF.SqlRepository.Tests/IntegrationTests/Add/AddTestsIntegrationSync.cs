using System;
using System.Linq;
using Shouldly;
using Xunit;
namespace Mkb.EF.SqlRepository.Tests.IntegrationTests.Add
{
    public class AddTestsIntegrationSync : BaseIntegrationSyncTests
    {
        [Fact]
        public void Ensure_add_works()
        {
            var testGuid = Guid.NewGuid().ToString("N");
            var email = $"{testGuid}@test.com";
            Sut.Add(new User {Name = testGuid, Email = email});
            Sut.Save();

            var item = SimpleDbContext.Users.FirstOrDefault(f => f.Email == email);
            item.Name.ShouldBe(testGuid);
        }
    }
}