using MicroservicesUser.DataAccess.Data;
using MicroservicesUser.DataAccess.Repository.Interfaces;
using MicroservicesUser.Models.DTO;
using MicroservicesUser.Models.Enums;
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

        public Task<AdminDashboardDTO> GetAdminDashboardAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ChartDTO> GetEmailVerificationChartByRangeAsync(int userId, string range)
        {
            List<int> scans = new();
            List<string> labels = new();
            DateTime now = DateTime.Now;
            DateTime today = DateTime.Today;

            IQueryable<EmailVerification> allScans = _dbContext.EmailVerifications
                .Where(e => e.UserId == userId);

            if (range == "today")
            {
                int currentInterval = (int)((now - today).TotalHours / 2);
                var intervalCounts = Enumerable.Range(0, currentInterval + 1)
                    .Select(i =>
                    {
                        DateTime intervalStart = today.AddHours(i * 2);
                        DateTime intervalEnd = intervalStart.AddHours(2);
                        string label = $"{intervalStart:hh:mm tt}-{intervalEnd.AddMinutes(-1):hh:mm tt}";
                        int count = allScans.Count(ev =>
                            ev.CreatedAt >= intervalStart && ev.CreatedAt < intervalEnd);
                        return new { label, count };
                    })
                    .ToList();

                labels = intervalCounts.Select(x => x.label).ToList();
                scans = intervalCounts.Select(x => x.count).ToList();
            }
            else if (range == "currentMonth")
            {

                var daysInMonth = DateTime.DaysInMonth(now.Year, now.Month);
                for (int day = 1; day <= daysInMonth; day++)
                {
                    DateTime date = new(now.Year, now.Month, day);
                    DateTime nextDate = date.AddDays(1);
                    string label = date.ToString("dd MMM");
                    int count = await _dbContext.EmailVerifications
                        .Where(ev => ev.UserId == userId && ev.CreatedAt >= date && ev.CreatedAt < nextDate)
                        .CountAsync();
                    labels.Add(label);
                    scans.Add(count);
                }
            }
            else if (range == "monthly")
            {

                for (int month = 1; month <= 12; month++)
                {
                    DateTime monthStart = new(now.Year, month, 1);
                    DateTime monthEnd = monthStart.AddMonths(1);
                    string label = monthStart.ToString("MMM");
                    int count = await _dbContext.EmailVerifications
                        .Where(ev => ev.UserId == userId && ev.CreatedAt >= monthStart && ev.CreatedAt < monthEnd)
                        .CountAsync();
                    labels.Add(label);
                    scans.Add(count);
                }
            }
            else if (range == "yearly")
            {

                int startYear = now.Year - 4;
                for (int year = startYear; year <= now.Year; year++)
                {
                    DateTime yearStart = new(year, 1, 1);
                    DateTime yearEnd = yearStart.AddYears(1);
                    string label = year.ToString();
                    int count = await _dbContext.EmailVerifications
                        .Where(ev => ev.UserId == userId && ev.CreatedAt >= yearStart && ev.CreatedAt < yearEnd)
                        .CountAsync();
                    labels.Add(label);
                    scans.Add(count);
                }
            }

            return new ChartDTO
            {
                Labels = labels,
                Scans = scans
            };
        }

        public async Task<EmailVerificationDashboardDTO> GetEmailVerificationDashboardAsync(int userId)
        {
            DateTime today = DateTime.Today;
            DateTime now = DateTime.Now;

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

            int currentInterval = (int)((now - today).TotalHours / 2);

            List<EmailVerification> todayVerifications = await _dbContext.EmailVerifications
            .Where(u => u.UserId == userId && u.CreatedAt >= today)
            .ToListAsync();

            var intervalCounts = Enumerable.Range(0, currentInterval + 1)
                .Select(i =>
                {
                    DateTime intervalStart = today.AddHours(i * 2);
                    DateTime intervalEnd = intervalStart.AddHours(2);
                    string label = $"{intervalStart:hh:mm tt}-{intervalEnd.AddMinutes(-1):hh:mm tt}";
                    int count = todayVerifications.Where(u => u.UserId == userId).Count(ev =>
                        ev.CreatedAt >= intervalStart && ev.CreatedAt < intervalEnd);
                    return new { label, count };
                })
                .ToList();

            List<string> labels = intervalCounts.Select(x => x.label).ToList();
            List<int> scans = intervalCounts.Select(x => x.count).ToList();

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

            return new EmailVerificationDashboardDTO
            {
                TotalCount = counts?.TotalCount ?? 0,
                TodayCount = counts?.TodayCount ?? 0,
                ValidCount = counts?.ValidCount ?? 0,
                SuccessRate = successRate,
                EmailVerificationList = emailVerificationList,
                ChartData = new ChartDTO
                {
                    Scans = scans,
                    Labels = labels
                }
            };
        }

        // public Task<AdminDashboardDTO> GetAdminDashboardAsync()
        // {
        //     DateTime today = DateTime.Today;
        // }

    }
}
