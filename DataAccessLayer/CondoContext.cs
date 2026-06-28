using CommonDataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class CondoContext : DbContext
    {
        public CondoContext(DbContextOptions<CondoContext> options)
            : base(options) {}

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Resident> Residents { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<Notification> Notification {  get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Account - PhoneNumber (Unique)
            modelBuilder.Entity<Account>()
                .HasIndex(b => b.PhoneNumber).IsUnique();

            // Apartment - Resident (1-nhiều)
            modelBuilder.Entity<Resident>()
                .HasOne(r => r.Apartment)
                .WithMany(a => a.Residents)
                .HasForeignKey(r => r.ApartmentId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Resident>()
                .HasOne(r => r.Account)
                .WithOne(a => a.Resident)
                .HasForeignKey<Resident>(r => r.AccountId);

            // Apartment - Contract (1-nhiều)
            modelBuilder.Entity<Contract>()
                .HasOne(c => c.Apartment)
                .WithMany(a => a.Contracts)
                .HasForeignKey(c => c.ApartmentId)
                .OnDelete(DeleteBehavior.NoAction);

            // Account - Resident (1-1)
            modelBuilder.Entity<Resident>()
                .HasIndex(r => r.AccountId).IsUnique();

            // Resident - Contract (1-nhiều)
            modelBuilder.Entity<Contract>()
                .HasOne(c => c.Resident)
                .WithMany(r => r.Contracts)
                .HasForeignKey(c => c.ResidentId)
                .OnDelete(DeleteBehavior.NoAction);

            // Resident - Payment (1-nhiều)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Resident)
                .WithMany(r => r.Payments)
                .HasForeignKey(p => p.ResidentId)
                .OnDelete(DeleteBehavior.NoAction);

            // Resident - Booking (1-nhiều)
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Resident)
                .WithMany(r => r.Bookings)
                .HasForeignKey(b => b.ResidentId)
                .OnDelete(DeleteBehavior.NoAction);

            // Service - Booking (1-nhiều)
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Service)
                .WithMany(s => s.Bookings)
                .HasForeignKey(b => b.ServiceId)
                .OnDelete(DeleteBehavior.NoAction);

            // Apartment - Incident (1-nhiều)
            modelBuilder.Entity<Incident>()
                .HasOne(i => i.Apartment)
                .WithMany(a => a.Incidents)
                .HasForeignKey(i => i.ApartmentId)
                .OnDelete(DeleteBehavior.NoAction);

            // Resident - Incident (1-nhiều)
            modelBuilder.Entity<Incident>()
                .HasOne(i => i.Resident)
                .WithMany(r => r.Incidents)
                .HasForeignKey(i => i.ReportedBy)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }
    }
}
