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
        public DbSet<ContractResident> ContractResidents { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<Notification> Notifications {  get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Ward> Wards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Account - IdentityNumber (Unique)
            modelBuilder.Entity<Account>()
                 .HasIndex(b => b.IdentityNumber).IsUnique();

            modelBuilder.Entity<Account>()
                .HasOne(a => a.Province)
                .WithMany(w => w.Accounts)
                .HasForeignKey(a => a.ProvinceId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Account>()
                .HasOne(a => a.Ward)
                .WithMany(w => w.Accounts)
                .HasForeignKey(a => a.WardId)
                .OnDelete(DeleteBehavior.NoAction);

            // Apartment - Contract (1-nhiều)
            modelBuilder.Entity<Contract>()
                .HasOne(c => c.Apartment)
                .WithMany(a => a.Contracts)
                .HasForeignKey(c => c.ApartmentId)
                .OnDelete(DeleteBehavior.NoAction);

            // Account - Resident (1-1)
            modelBuilder.Entity<Resident>()
                .HasIndex(r => r.AccountId).IsUnique();

            modelBuilder.Entity<Resident>()
                .HasOne(r => r.Account)
                .WithOne(a => a.Resident)
                .HasForeignKey<Resident>(r => r.AccountId);

            // Khai báo composite key cho bảng trung gian
            modelBuilder.Entity<ContractResident>()
                .HasKey(cr => new { cr.ContractId, cr.ResidentId });

            // Quan hệ Contract ↔ ContractResident
            modelBuilder.Entity<ContractResident>()
                .HasOne(cr => cr.Contract)
                .WithMany(c => c.ContractResidents)
                .HasForeignKey(cr => cr.ContractId)
                .OnDelete(DeleteBehavior.NoAction);

            // Quan hệ Resident ↔ ContractResident
            modelBuilder.Entity<ContractResident>()
                .HasOne(cr => cr.Resident)
                .WithMany(r => r.ContractResidents)
                .HasForeignKey(cr => cr.ResidentId)
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
