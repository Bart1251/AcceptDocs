using AcceptDocs.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AcceptDocs.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<PositionLevel> PositionLevels { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AcceptanceRequest> AcceptanceRequests { get; set; }
        public DbSet<DocumentFlow> DocumentFlows { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
