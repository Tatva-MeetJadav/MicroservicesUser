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
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email && x.IsDeleted == false);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Username == username && x.IsDeleted == false);
        }

        public async Task<User?> GetByUsernameAndNotById(string username, int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Username == username && x.Id != id && x.IsDeleted == false);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
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
                users = users.Where(x => x.Email.ToLower().Contains(paginationDTO.SearchQuery.ToLower()) || x.FirstName.ToLower().Contains(paginationDTO.SearchQuery.ToLower()) || x.LastName.ToLower().Contains(paginationDTO.SearchQuery.ToLower())).ToList();
            }
            if (paginationDTO.OrderOfSorting == "asc")
            {
                if (paginationDTO.ColumnNameForSorting == "User")
                {
                    users = users.OrderBy(u => u.FirstName + u.LastName).ToList();
                }
                else if (paginationDTO.ColumnNameForSorting == "Email")
                {
                    users = users.OrderBy(u => u.Email).ToList();
                }
                else if (paginationDTO.ColumnNameForSorting == "Status")
                {
                    users = users.OrderBy(u => u.IsBlocked).ThenBy(u => u.IsDeleted).ToList();
                }
                else
                {
                    users = users.OrderByDescending(u => u.CreatedAt).ToList();
                }
            }
            else
            {
                if (paginationDTO.ColumnNameForSorting == "User")
                {
                    users = users.OrderByDescending(u => u.FirstName + u.LastName).ToList();
                }
                else if (paginationDTO.ColumnNameForSorting == "Email")
                {
                    users = users.OrderByDescending(u => u.Email).ToList();
                }
                else if (paginationDTO.ColumnNameForSorting == "Status")
                {
                    users = users.OrderByDescending(u => u.IsDeleted).ThenByDescending(u => u.IsBlocked).ToList();
                }
            }
            int totalCount = users.Count;
            users = users.Skip((paginationDTO.CurrentPage - 1) * paginationDTO.PageSize).Take(paginationDTO.PageSize).ToList();

            return (users, totalCount);
        }
    }
}
