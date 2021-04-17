using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace Mkb.EF.SqlRepository.Tests.IntegrationTests.GetAll
{
    public class GetAllIntegrationAsync : BaseIntegrationAsyncTests
    {
        private List<User> WhatWeShouldHave()
        {
            var users = Enumerable.Range(1, 10).Select(f => Guid.NewGuid().ToString("N"))
                .Select(f => new User
                {
                    Name = f, Email = $"{f}@gmail.com",
                    Posts = new List<Post>() {new Post {CreatedAt = DateTime.Now, Text = f + " Text"}}
                }).ToList();

            SimpleDbContext.Users.AddRange(users);
            SimpleDbContext.SaveChanges();
            return users;
        }

        [Fact]
        public async Task Ensure_we_can_GetAll()
        {
            var users = WhatWeShouldHave();
            var all = Sut.GetAll<User>().ToList();

            users.All(f => all.Select(t => t.Email).Contains(f.Email)).ShouldBeTrue();
        }

        [Fact]
        public async Task Ensure_we_can_GetAll_with_where()
        {
            var users = WhatWeShouldHave();
            var all = Sut.GetAll<User>(f => f.Email == users.First().Email).ToList();

            all.Count().ShouldBe(1);
            all.First().ShouldBe(users.First());
        }

        [Fact]
        public async Task Ensure_we_can_GetAll_with_where_and_projection()
        {
            var users = WhatWeShouldHave();
            var all = Sut.GetAll<User, string>(f => f.Email == users.First().Email, f => f.Email).ToList();

            all.Count().ShouldBe(1);
            all.First().ShouldBe(users.First().Email);
        }

        [Fact]
        public async Task Ensure_we_can_GetAll_with_includes()
        {
            var users = WhatWeShouldHave();
            var all = Sut.GetAll<User, User>(f => f.Email == users.First().Email, f => f, true, f => f.Posts).ToList();

            all.Count().ShouldBe(1);
            all.First().Posts.First().ShouldBe(users.First().Posts.First());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Ensure_we_test_track(bool track)
        {
            var users = WhatWeShouldHave();
            var item = Sut.GetAll<User, User>(f => f.Email == users.First().Email, f => f, track).First();

            item.Email = "changed";
            await Sut.Save();

            var returned = await SimpleDbContext.Users.FirstOrDefaultAsync(f => f.Id == item.Id);

            if (track)
            {
                returned.Email.ShouldBe("changed");
                return;
            }

            returned.Email.ShouldContain(returned.Name);
        }
    }
}