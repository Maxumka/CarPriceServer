using Microsoft.EntityFrameworkCore;
using CarMarkTypeAPI.Database.Entity;

namespace CarMarkTypeAPI.Database
{
    public class CarMarkTypeContext : DbContext
    {
        public DbSet<CarMark> CarMarks { get; set; }

        public DbSet<CarType> CarTypes { get; set; }

        public CarMarkTypeContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");

            modelBuilder.Entity<CarMark>().ToTable("CarMark");
            modelBuilder.Entity<CarMark>().HasKey(c => c.Id);
            modelBuilder.Entity<CarMark>().Property(c => c.Name);

            modelBuilder.Entity<CarType>().ToTable("CarType");
            modelBuilder.Entity<CarType>().HasKey(c => c.Id);
            modelBuilder.Entity<CarType>().Property(c => c.Name);
            modelBuilder.Entity<CarType>().Property(c => c.Link);
            modelBuilder.Entity<CarType>().HasOne(c => c.CarMark)
                                          .WithMany(c => c.CarTypes)
                                          .HasForeignKey(c => c.CarMarkId);

            base.OnModelCreating(modelBuilder);
        }
    }
}