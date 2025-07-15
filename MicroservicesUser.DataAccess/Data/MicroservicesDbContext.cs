using MicroservicesUser.Models.Models;
using Microsoft.EntityFrameworkCore;


namespace MicroservicesUser.DataAccess.Data
{
    public class MicroservicesDbContext : DbContext
    {
        public MicroservicesDbContext(DbContextOptions<MicroservicesDbContext> options) : base(options) { }
        public DbSet<Log> Logs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.GetTableName()?.ToLower() ?? string.Empty);
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.Name.ToLower());
                }
            }
        }
    }


}

//Add-Migration InitialCreate -Project MicroservicesUser.Migrations -StartupProject MicroservicesUser.Web -Context MicroservicesUserDbContext
//Update-Database -Project MicroservicesUser.Migrations -StartupProject MicroservicesUser.Web -Context MicroservicesUserDbContext