using AcceptDocs.Domain.Contracts;
using AcceptDocs.Domain.Models;
using AcceptDocs.SharedKernel.Dto;

namespace AcceptDocs.Infrastructure.Repositories
{
    public class PositionLevelRepository : Repository<PositionLevel>, IPositionLevelRepository
    {
        public PositionLevelRepository(AppDbContext context) : base(context)
        {
            
        }
    }
}
