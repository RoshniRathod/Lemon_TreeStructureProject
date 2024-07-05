using Microsoft.EntityFrameworkCore;
using TreeStructures.Models;

namespace TreeStructures.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
        }

        public DbSet<NodeInfo> Nodes { get; set; }
    }
}
