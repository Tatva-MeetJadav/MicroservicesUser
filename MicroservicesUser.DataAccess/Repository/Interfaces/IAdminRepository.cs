using MicroservicesUser.Models.Models;

namespace MicroservicesUser.DataAccess.Repository.Interfaces
{
    public interface IAdminRepository
    {
        Task<Admin?> GetByEmailAsync(string email);
        Task UpdateAsync(Admin admin);
        Task<Admin?> GetByResetPasswordToken(string resetPasswordToken);
        Task<Admin?> GetByIdAsync(int id);
    }
}