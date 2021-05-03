using IdentityAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentityAPI.Repository.Contexts
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UserContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");

            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().Property(u => u.Login);
            modelBuilder.Entity<User>().Property(u => u.Password);

            base.OnModelCreating(modelBuilder);
        }
    }
}
