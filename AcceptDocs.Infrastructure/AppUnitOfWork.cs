using AcceptDocs.Domain.Contracts;

namespace AcceptDocs.Infrastructure
{
    public class AppUnitOfWork : IAppUnitOfWork
    {
        private readonly AppDbContext _context;

        //repositories

        public AppUnitOfWork(AppDbContext context)//repositories
        {
            _context = context;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
