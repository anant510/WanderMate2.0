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
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<PasswordReset> PasswordResets { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


             modelBuilder.Entity<Booking>()
            .HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);
 
            modelBuilder.Entity<Booking>()
            .HasOne(b => b.Hotel)
            .WithMany(h => h.Bookings)
            .HasForeignKey(b => b.HotelId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Review>()
            .HasOne(r => r.Hotel)
            .WithMany(h => h.Reviews)
            .HasForeignKey(r => r.HotelId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PasswordReset>()
            .Property(pr => pr.Token)
            .IsRequired(); // or any other configuration needed

            
    
        // Configure the one-to-many relationship
        // modelBuilder.Entity<Hotel>()
        //     .HasMany(h => h.Reviews)
        //     .WithOne(r => r.Hotel)
        //     .HasForeignKey(r => r.HotelId)
        //     .OnDelete(DeleteBehavior.Cascade); // Optional: Configure cascade delete

        // Apply global query filter to exclude soft-deleted entities
        modelBuilder.Entity<Hotel>().HasQueryFilter(h => !h.IsDeleted);
        }
    }
}