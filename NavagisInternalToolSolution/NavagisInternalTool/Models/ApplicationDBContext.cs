using System.Data.Entity;

namespace NavagisInternalTool.Models
{
    public class ApplicationDBContext : DbContext
    {
        public DbSet<Setting> Setting { get; set; }
        public DbSet<User> Users { get; set; }
    }
}