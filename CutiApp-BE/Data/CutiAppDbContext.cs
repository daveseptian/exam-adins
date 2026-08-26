using CutiApp.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CutiApp.Data
{
    public class CutiAppDbContext: DbContext
    {
        public CutiAppDbContext(DbContextOptions<CutiAppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<LeaveBalance> LeaveBalances { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach(var entity in modelBuilder.Model.GetEntityTypes())
            {
                modelBuilder.Entity<User>()
                    .HasIndex(u => u.Username)
                    .IsUnique();

                modelBuilder.Entity<LeaveBalance>()
                    .HasOne(lb => lb.User)
                    .WithOne(u => u.LeaveBalance)
                    .OnDelete(DeleteBehavior.Restrict);

                modelBuilder.Entity<LeaveRequest>()
                    .HasOne(lr => lr.User)
                    .WithMany(u => u.LeaveRequests)
                    .HasForeignKey(lr => lr.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            }
        }
    }
}
