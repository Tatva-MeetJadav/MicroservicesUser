using MicroservicesUser.Models.Models;
using Microsoft.EntityFrameworkCore;


namespace MicroservicesUser.DataAccess.Data
{
    public class MicroservicesUserDbContext : DbContext
    {
        public MicroservicesUserDbContext(DbContextOptions<MicroservicesUserDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<EmailVerification> EmailVerifications { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmailVerification>()
                .Property(e => e.Status)
                .HasConversion<string>();
        }
    }


}

//Add-Migration InitialCreate -Project MicroservicesUser.Migrations -StartupProject MicroservicesUser.Web -Context MicroservicesUserDbContext
//Update-Database -Project MicroservicesUser.Migrations -StartupProject MicroservicesUser.Web -Context MicroservicesUserDbContext