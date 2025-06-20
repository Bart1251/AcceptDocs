﻿using AcceptDocs.Domain.Contracts;
using AcceptDocs.Domain.Models;
using AcceptDocs.SharedKernel.Dto;
using Microsoft.EntityFrameworkCore;

namespace AcceptDocs.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {

        }

        public List<User> GetAllWithPositionLevel()
        {
            return _context.Users.Include(u => u.PositionLevel).ToList();
        }

        public bool IsLoginUsed(string login)
        {
            return _context.Users.Any(u => u.Login == login);
        }

        public bool IsLoginUsed(string login, int id)
        {
            return _context.Users.Any(u => u.Login == login && u.UserId != id);
        }

        public User GetWithPositionLevel(int id)
        {
            return _context.Users.Include(u => u.PositionLevel).Where(u => u.UserId == id).FirstOrDefault()!;
        }

        public bool CanDeleteUser(int id)
        {
            var user = _context.Users.Include(u => u.AcceptanceRequests).FirstOrDefault(u => u.UserId == id);
            if (user == null)
                return true;
            return user.AcceptanceRequests.Count(ar => ar.AcceptanceRequestStatus == AcceptanceRequestStatus.NotAnswered) == 0;
        }
    }
}
