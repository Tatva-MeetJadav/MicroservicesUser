using MicroservicesUser.Models.Models;

namespace MicroservicesUser.DataAccess.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByPasswordResetToken(string token);
    }
}
