using MicroservicesUser.DataAccess.Data;
using MicroservicesUser.DataAccess.Repository.Interfaces;
using MicroservicesUser.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroservicesUser.DataAccess.Repository.Implementations
{
    public class AdminRepository : IAdminRepository
    {
        private readonly MicroservicesUserDbContext _context;
        public AdminRepository(MicroservicesUserDbContext context)
        {
            _context = context;
        }
        public async Task<Admin?> GetByEmailAsync(string email)
        {
            return await _context.Admins.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<Admin?> GetByResetPasswordToken(string resetPasswordToken)
        {
            Admin? admin = await _context.Admins.FirstOrDefaultAsync(x => x.PasswordResetToken == resetPasswordToken);
            return admin;
        }

        public async Task UpdateAsync(Admin admin)
        {
            _context.Admins.Update(admin);
            await _context.SaveChangesAsync();
        }

        public async Task<Admin?> GetByIdAsync(int id)
        {
            return await _context.Admins.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}