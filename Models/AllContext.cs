using Microsoft.EntityFrameworkCore;
 
namespace productcategories.Models
{
    public class AllContext : DbContext
    {
        public AllContext (DbContextOptions<AllContext> options) : base(options) {}
        public DbSet<Categories> categories { get; set; }
        public DbSet<Products> products { get; set; }
        public DbSet<Associations> associations { get; set; }


    }
}