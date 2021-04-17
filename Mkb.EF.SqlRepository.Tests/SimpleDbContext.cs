using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Mkb.EF.SqlRepository.Tests
{
    public class SimpleDbContext : DbContext
    {
        // obviously you would not have this here but as its a test meh
        const string DbConnectionString = "Server=192.168.0.204;Database=TestDbToUse;User Id=sa;Password=12345678;";

        public SimpleDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DbConnectionString);
            base.OnConfiguring(optionsBuilder);
        }


        public SimpleDbContext(DbContextOptions<SimpleDbContext> options)
            : base(options)
        {
        }


        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(user =>
            {
                user.Property(x => x.Email)
                    .HasMaxLength(200)
                    .IsRequired()
                    .IsUnicode(false);

                user.Property(f => f.Id).IsRequired().HasDefaultValueSql("NEWID()");
                user.HasMany(f => f.Posts).WithOne(x => x.User)
                    .HasForeignKey(x => x.UserId)
                    .HasConstraintName("FK_Users_Posts");
            });


            modelBuilder.Entity<Post>(posts =>
            {
                posts.Property(f => f.Id).UseIdentityColumn(1, 1);

                posts.HasOne(f => f.User).WithMany(x => x.Posts)
                    .HasForeignKey(x => x.UserId)
                    .HasConstraintName("FK_Users_Posts");
            });
        }
    }

    public class Post
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }

    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}