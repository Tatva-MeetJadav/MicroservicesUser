using MicroservicesUser.DataAccess.Data;
using MicroservicesUser.DataAccess.Repository.Interfaces;
using MicroservicesUser.Models.DTO;
using MicroservicesUser.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroservicesUser.DataAccess.Repository.Implementations
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly MicroservicesUserDbContext _dbContext;
        public DashboardRepository(MicroservicesUserDbContext _DbContext)
        {
            _dbContext = _DbContext;
        }
        public async Task<DashboardDTO> GetDashboardAsync(int userId)
        {
            DateTime today = DateTime.Today;
            var counts = await _dbContext.EmailVerifications
                .Where(u => u.UserId == userId)
                .GroupBy(u => u.UserId)
                .Select(g => new
                {
                    TotalCount = g.Count(),
                    TodayCount = g.Count(u => u.CreatedAt >= today),
                    ValidCount = g.Count(u => EF.Functions.JsonContains(u.EmailResponseParam, "{\"valid\": true}")),
                    SuccessCount = g.Count(u => u.Status == Status.Success)
                })
                .FirstOrDefaultAsync();


            double successRate = 0;
            if (counts != null && counts.TotalCount > 0)
            {
                successRate = counts.SuccessCount * 100.0 / counts.TotalCount;
            }

            List<EmailVerification> emailVerifications = await _dbContext.EmailVerifications
                .Where(u => u.UserId == userId)
                .OrderByDescending(u => u.CreatedAt)
                .Take(5)
                .ToListAsync();

            List<EmailVerificationDTO> emailVerificationList = emailVerifications.Select(x => new EmailVerificationDTO
            {
                Email = x.EmailRequestParam.RootElement.GetProperty("Email").ToString(),
                ResponseStatus = x.Status.ToString()
            }).ToList();

            return new DashboardDTO
            {
                TotalCount = counts?.TotalCount ?? 0,
                TodayCount = counts?.TodayCount ?? 0,
                ValidCount = counts?.ValidCount ?? 0,
                SuccessRate = successRate,
                EmailVerificationList = emailVerificationList,
            };
        }
    }
}
