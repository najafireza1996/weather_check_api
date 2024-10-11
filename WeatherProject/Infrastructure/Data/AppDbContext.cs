using System;
using Microsoft.EntityFrameworkCore;
using WeatherProject.Domain.Entities;

namespace WeatherProject.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Weather> Weathers { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
     protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Weather>()
                .HasKey(w => w.Id);

            modelBuilder.Entity<Weather>()
                .Property(w => w.Temperature2m)
                .IsRequired();
        }
    }
}

