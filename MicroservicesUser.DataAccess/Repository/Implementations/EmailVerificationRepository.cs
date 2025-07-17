using System.Text.Json;
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

        public async Task<AdminEmailVerificationDashboardDTO> GetEmailVerificationHistoryList(AdminEmailVerificationHistoryRequestDTO requestDto)
        {
            List<EmailVerification> data = await _dbContext.EmailVerifications
            .Include(u => u.User)
            .Where(u => requestDto.UserIds!.Count == 0 || requestDto.UserIds == null || requestDto.UserIds.Contains(u.UserId))
            .ToListAsync();

            if (!string.IsNullOrEmpty(requestDto.PaginationDTO!.SearchQuery))
            {
                data = data.Where(x => x.User!.Username!.ToLower().Contains(requestDto.PaginationDTO.SearchQuery.ToLower()) || x.EmailRequestParam.RootElement.GetProperty("Email").ToString().ToLower().Contains(requestDto.PaginationDTO.SearchQuery.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(requestDto.ScannedStatus))
            {
                data = data.Where(x => x.Status.ToString() == requestDto.ScannedStatus).ToList();
            }

            if (requestDto.Valid != null)
            {
                data = data.Where(x => x.EmailResponseParam.RootElement.GetProperty("valid").GetBoolean() == requestDto.Valid).ToList();
            }
            List<EmailVerification> paginatedEntries = data
                .Skip((requestDto.PaginationDTO!.CurrentPage - 1) * requestDto.PaginationDTO.PageSize)
                .Take(requestDto.PaginationDTO.PageSize)
                .ToList();

            return new AdminEmailVerificationDashboardDTO
            {
                EmailVerificationHistoryList = paginatedEntries.Select(ev => new AdminEmailVerificationHistoryListDTO
                {
                    Id = ev.Id,
                    Username = ev.User?.Username ?? "N/A",
                    VerifiedEmail = ev.EmailResponseParam.RootElement.TryGetProperty("valid", out JsonElement emailProp) ? emailProp.ToString() : string.Empty,
                    FraudScore = Convert.ToInt16(ev.EmailResponseParam.RootElement.TryGetProperty("fraudScore", out JsonElement fraudScoreProp) ? fraudScoreProp.ToString() : "0"),
                    ScannedStatus = ev.Status.ToString(),
                    Valid = ev.EmailResponseParam.RootElement.TryGetProperty("valid", out JsonElement validProp) ? validProp.GetBoolean() : false
                }).ToList()
            };
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

            if (paginationVM.ColumnNameForFilter == "Valid" && paginationVM.FilterValue == "True")
            {
                result = result.Where(u =>
                u.EmailResponseParam.RootElement.TryGetProperty("valid", out JsonElement validProp) &&
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