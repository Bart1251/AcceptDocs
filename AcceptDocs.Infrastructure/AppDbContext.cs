using Microsoft.EntityFrameworkCore;

namespace AcceptDocs.Infrastructure
{
    public class AppDbContext : DbContext
    {
        //public DbSet<!!SET!!> !!SET!! { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
