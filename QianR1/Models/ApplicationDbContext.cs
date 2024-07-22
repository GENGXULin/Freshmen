using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace COMP3851B.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<FAQs> FAQs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Announcements> Announcements { get; set; }
        public DbSet<Log> Logs { get; set; }
    }
}
