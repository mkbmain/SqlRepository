using System;
using System.Linq;
using Shouldly;
using Xunit;

namespace Mkb.EF.SqlRepository.Tests.IntegrationTests.AddAndSave
{
    public class AddAndSaveTestsIntegrationSync : BaseIntegrationSyncTests
    {
        [Fact]
        public void Ensure_addAndSave_works()
        {
            var testGuid = Guid.NewGuid().ToString("N");
            var email = $"{testGuid}@test.com";
            Sut.AddAndSave(new User {Name = testGuid, Email = email});

            var item = SimpleDbContext.Users.FirstOrDefault(f => f.Email == email);
            item.Name.ShouldBe(testGuid);
        }
    }
}