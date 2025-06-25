using System.Text.Json;
using MicroservicesUser.DataAccess.Data;
using MicroservicesUser.DataAccess.Repository.Interfaces;
using MicroservicesUser.Models.Models;
using MicroservicesUser.Models.ViewModels;
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

        public async Task<EmailVerification> GetAsync(int id)
        {
            EmailVerification? result = await _dbContext.EmailVerifications.FirstOrDefaultAsync(x => x.Id == id);
            return result!;
        }

        public async Task<(List<EmailVerification>, int)> GetListByUserId(int userId, PaginationVM paginationVM)
        {
            List<EmailVerification>? result = await _dbContext.EmailVerifications
            .Where(x => x.UserId == userId)
            .OrderBy(x => x.Id)
            .ToListAsync();

            if (!string.IsNullOrEmpty(paginationVM.SearchQuery) && !string.IsNullOrWhiteSpace(paginationVM.SearchQuery))
            {
                result = result.Where(x => x.EmailRequestParam.RootElement.GetProperty("Email").GetString()!.ToLower().Contains(paginationVM.SearchQuery.ToLower())).ToList();
            }

            if (paginationVM.ColumnNameForFilter == "Valid" && paginationVM.FilterValue)
            {
                result = result.Where(u =>
                u.EmailResponseParam.RootElement.TryGetProperty("valid", out var validProp) &&
                validProp.GetBoolean()).ToList();
            }

            int count = result.Count;
            result = result
            .Skip((paginationVM.CurrentPage - 1) * paginationVM.PageSize)
            .Take(paginationVM.PageSize)
            .ToList();

            if (paginationVM.ColumnNameForSorting == "CreatedAt")
            {
                if (paginationVM.OrderOfSorting == "asc")
                {
                    result = result.OrderBy(x => x.CreatedAt).ToList();
                }
                else
                {
                    result = result.OrderByDescending(x => x.CreatedAt).ToList();
                }
            }
            return (result, count);
        }

        public async Task<int> GetCountByUserId(int userId)
        {
            int count = await _dbContext.EmailVerifications.Where(u => u.UserId == userId).CountAsync();
            return count;
        }

        public async Task<int> GetCountBetweenDate(int userId, DateTime fromDate, DateTime toDate)
        {
            int count = await _dbContext.EmailVerifications.Where(u => u.CreatedAt >= fromDate && u.CreatedAt <= toDate && u.UserId == userId).CountAsync();
            return count;
        }

        public async Task<int> GetCountOfValidEmails(int userId)
        {
            int count = await _dbContext.EmailVerifications
            .Where(u => u.UserId == userId && EF.Functions.JsonContains(u.EmailResponseParam, "{\"valid\": true}")
            )
            .CountAsync();
            return count;
        }
    }
}