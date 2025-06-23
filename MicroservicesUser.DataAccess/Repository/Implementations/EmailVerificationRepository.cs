using System.Text.Json;
using MicroservicesUser.DataAccess.Data;
using MicroservicesUser.DataAccess.Repository.Interfaces;
using MicroservicesUser.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroservicesUser.DataAccess.Repository.Implementations
{
    public class EmailVerificationRepository : IEmailVerificationRepository
    {
        private readonly MicroservicesUserDbContext _dbContext;
        public EmailVerificationRepository(MicroservicesUserDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(EmailVerification emailVerification)
        {
            await _dbContext.AddAsync(emailVerification);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<(List<EmailVerification>, int)> GetListByUserId(int userId, int page, int pageSize, string searchQuery)
        {
            List<EmailVerification>? result = await _dbContext.EmailVerifications
            .Where(x => x.UserId == userId)
            .OrderBy(x => x.Id)
            .ToListAsync();

            result = result
            .Where(x => x.EmailRequestParam.RootElement.GetProperty("Email").GetString() == searchQuery || string.IsNullOrEmpty(searchQuery)).ToList();

            int count = result.Count;
            result = result
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
            return (result, count);
        }
    }
}