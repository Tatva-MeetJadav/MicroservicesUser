using MicroservicesUser.DataAccess.Data;
using MicroservicesUser.DataAccess.Repository.Interfaces;
using MicroservicesUser.Models.DTO;
using MicroservicesUser.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroservicesUser.DataAccess.Repository.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly MicroservicesUserDbContext _context;
        public UserRepository(MicroservicesUserDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync(); ;
        }
        public async Task<User?> GetByPasswordResetToken(string token)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.PasswordResetToken == token);
        }

        public async Task<(List<User>, int)> GetListAsync(PaginationDTO paginationDTO)
        {
            List<User> users = await _context.Users.ToListAsync();

            if (!string.IsNullOrEmpty(paginationDTO.SearchQuery))
            {
                users = users.Where(x => x.Email.ToLower().Contains(paginationDTO.SearchQuery.ToLower())).ToList();
            }

            int totalCount = users.Count;
            users = users.Skip((paginationDTO.CurrentPage - 1) * paginationDTO.PageSize).Take(paginationDTO.PageSize).ToList();
            return (users, totalCount);
        }
    }
}
