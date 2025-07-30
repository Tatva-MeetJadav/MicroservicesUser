using MicroservicesUser.Models.Models;
using Microsoft.EntityFrameworkCore;


namespace MicroservicesUser.DataAccess.Data
{
    public class MicroservicesUserDbContext : DbContext
    {
        public MicroservicesUserDbContext(DbContextOptions<MicroservicesUserDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<EmailVerification> EmailVerifications { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<ProxyVpnDetection> ProxyVpnDetections { get; set; }
        public DbSet<HelpAndSupport> HelpAndSupports { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmailVerification>()
                .Property(e => e.Status)
                .HasConversion<string>();

            modelBuilder.Entity<ProxyVpnDetection>()
                .Property(e => e.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Admin>()
               .Property(e => e.Role)
               .HasConversion<string>();
        }
    }
}

//Add-Migration InitialCreate -Project MicroservicesUser.Migrations -StartupProject MicroservicesUser.Web -Context MicroservicesUserDbContext
//Update-Database -Project MicroservicesUser.Migrations -StartupProject MicroservicesUser.Web -Context MicroservicesUserDbContext