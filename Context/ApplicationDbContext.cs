using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using usingLinq.Models;

namespace usingLinq.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Review> Reviews { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        // Configure the one-to-many relationship
        modelBuilder.Entity<Hotel>()
            .HasMany(h => h.Reviews)
            .WithOne(r => r.Hotel)
            .HasForeignKey(r => r.HotelId)
            .OnDelete(DeleteBehavior.Cascade); // Optional: Configure cascade delete

        // Apply global query filter to exclude soft-deleted entities
        modelBuilder.Entity<Hotel>().HasQueryFilter(h => !h.IsDeleted);
        }
    }
}