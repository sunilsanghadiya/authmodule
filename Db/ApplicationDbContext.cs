using authmodule.Entitis;
using Microsoft.EntityFrameworkCore;

namespace authmodule.Db
{
    public class ApplicationDbContext : DbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option) {
            
        }

        public DbSet<TempData> TempData => Set<TempData>();
    }
}