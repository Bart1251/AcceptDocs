using AcceptDocs.Domain.Contracts;
using AcceptDocs.Domain.Models;
using AcceptDocs.SharedKernel.Dto;
using Microsoft.EntityFrameworkCore;

namespace AcceptDocs.Infrastructure.Repositories
{
    public class PositionLevelRepository : Repository<PositionLevel>, IPositionLevelRepository
    {
        public PositionLevelRepository(AppDbContext context) : base(context)
        {
            
        }

        public bool CanDelete(int id)
        {
            var positionLevel = _context.PositionLevels.Include(pl => pl.Users).FirstOrDefault(pl => pl.PositionLevelId == id);
            if (positionLevel == null)
                return true;
            return positionLevel.Users.Count == 0;
        }
    }
}
