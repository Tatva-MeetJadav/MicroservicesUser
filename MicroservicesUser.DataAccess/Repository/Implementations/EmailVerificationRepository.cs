using System.Text.Json;
using AutoMapper;
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
        private readonly IMapper _mapper;
        public EmailVerificationRepository(MicroservicesUserDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
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

            if (!string.IsNullOrEmpty(requestDto.Valid))
            {
                if (requestDto.Valid == "Unknown")
                {
                    data = data.Where(x => !x.EmailResponseParam.RootElement.TryGetProperty("valid", out JsonElement validProp)).ToList();
                }
                else
                {
                    data = data.Where(x => x.EmailResponseParam.RootElement.TryGetProperty("valid", out JsonElement validProp) && (requestDto.Valid == validProp.ToString())).ToList();
                }

            }

            List<EmailVerification> paginatedEntries = data
                .Skip((requestDto.PaginationDTO!.CurrentPage - 1) * requestDto.PaginationDTO.PageSize)
                .Take(requestDto.PaginationDTO.PageSize)
                .ToList();

            AdminEmailVerificationDashboardDTO dashboardDTO = _mapper.Map<AdminEmailVerificationDashboardDTO>(requestDto.PaginationDTO);
            dashboardDTO.TotalItems = data.Count;
            dashboardDTO.EmailVerificationHistoryList = paginatedEntries.Select(ev => new AdminEmailVerificationHistoryListDTO
            {
                Id = ev.Id,
                Username = ev.User?.Username ?? "N/A",
                UserStatus = ev.User!.IsDeleted ? "Inactive" : ev.User.IsBlocked ? "Blocked" : "Active",
                VerifiedEmail = ev.EmailRequestParam.RootElement.TryGetProperty("Email", out JsonElement emailProp) ? emailProp.ToString() : string.Empty,
                FraudScore = Convert.ToInt16(ev.EmailResponseParam.RootElement.TryGetProperty("fraudScore", out JsonElement fraudScoreProp) ? fraudScoreProp.ToString() : "0"),
                ScannedStatus = ev.Status.ToString(),
                Valid = ev.EmailResponseParam.RootElement.TryGetProperty("valid", out JsonElement validProp) ? validProp.ToString() : string.Empty,
            }).ToList();

            return dashboardDTO;
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
            int count = result.Count;
            result = result
            .Skip((paginationVM.CurrentPage - 1) * paginationVM.PageSize)
            .Take(paginationVM.PageSize)
            .ToList();
            return (result, count);
        }
    }
}