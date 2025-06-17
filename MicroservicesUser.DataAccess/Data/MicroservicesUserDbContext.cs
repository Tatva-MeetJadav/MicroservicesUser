using MicroservicesUser.Models.Models;
using Microsoft.EntityFrameworkCore;


namespace MicroservicesUser.DataAccess.Data
{
    public class MicroservicesUserDbContext:DbContext
    {
        public MicroservicesUserDbContext(DbContextOptions<MicroservicesUserDbContext> options):base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
