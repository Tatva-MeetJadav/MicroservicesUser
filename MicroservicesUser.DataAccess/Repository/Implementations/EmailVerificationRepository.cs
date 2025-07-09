using MicroservicesUser.DataAccess.Data;
using MicroservicesUser.DataAccess.Repository.Interfaces;
using MicroservicesUser.Models.DTO;
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

        public async Task<EmailVerification> GetAsync(int id)
        {
            EmailVerification? result = await _dbContext.EmailVerifications.FirstOrDefaultAsync(x => x.Id == id);
            return result!;
        }

        public async Task<(List<EmailVerification>, int)> GetListByUserId(int userId, PaginationDTO paginationVM)
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
    }
}