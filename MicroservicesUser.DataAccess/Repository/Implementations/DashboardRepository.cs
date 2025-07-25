using System.Text.Json;
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

        public async Task<AdminEmailVerificationDashboardDTO> GetAdminEmailVerificationDashboardAsync(List<int>? userIds)
        {
            DateTime now = DateTime.Now;
            DateTime today = DateTime.Today;

            IQueryable<EmailVerification> baseQuery = _dbContext.EmailVerifications
                 .Where(u => userIds == null || userIds.Contains(u.UserId));

            List<User> users = _dbContext.Users.ToList();

            int currentInterval = (int)((now - today).TotalHours / 3);
            var intervalCounts = Enumerable.Range(0, currentInterval + 1)
                .Select(i =>
                {
                    DateTime intervalStart = today.AddHours(i * 3);
                    DateTime intervalEnd = intervalStart.AddHours(3);
                    string label = $"{intervalStart:hh:mm tt}";
                    int count = baseQuery.Count(ev =>
                        ev.CreatedAt >= intervalStart && ev.CreatedAt < intervalEnd);
                    return new { label, count };
                })
                .ToList();


            List<EmailVerificationDateTimeStatesDTO> ChartData = intervalCounts
            .Select(x => new EmailVerificationDateTimeStatesDTO
            {
                EmailVerificationCount = x.count,
                CreatedAt = x.label
            }).ToList();

            List<EmailVerification> allEmailVerifications = await baseQuery.ToListAsync();
            int validCount = allEmailVerifications.Count(ev =>
            ev.EmailResponseParam.RootElement.TryGetProperty("valid", out JsonElement validCountVal)
            && validCountVal.GetBoolean());

            int fraudScoreCount = allEmailVerifications.Count(ev =>
           ev.EmailResponseParam.RootElement.TryGetProperty("fraudScore", out JsonElement fraudScoreProp) && fraudScoreProp.TryGetInt32(out int fraudScore));
            int fraudScoreSum = allEmailVerifications.Sum(ev =>
            {
                if (ev.EmailResponseParam.RootElement.TryGetProperty("fraudScore", out JsonElement fraudScoreProp) &&
                    fraudScoreProp.TryGetInt32(out int fraudScore))
                {
                    return fraudScore;
                }
                return 0;
            });

            int totalItems = await baseQuery.CountAsync();
            List<EmailVerification> emailVerifications = await baseQuery
                .Take(5)
                .Include(ev => ev.User)
                .ToListAsync();
            List<AdminEmailVerificationHistoryListDTO> historyList = new();
            int successVerifications = baseQuery.Count(ev => ev.Status == Status.Success);
            foreach (EmailVerification ev in emailVerifications)
            {
                JsonElement responseJson = ev.EmailResponseParam.RootElement;
                JsonElement requestJson = ev.EmailRequestParam.RootElement;

                string? email = null;
                bool valid = false;
                int fraudScore = 0;

                if (requestJson.TryGetProperty("Email", out JsonElement emailProp))
                {
                    email = emailProp.GetString();
                }

                if (responseJson.TryGetProperty("valid", out JsonElement validProp))
                {
                    valid = validProp.GetBoolean();
                }

                if (responseJson.TryGetProperty("fraudScore", out JsonElement scoreProp))
                {
                    fraudScore = scoreProp.GetInt32();
                }
                AdminEmailVerificationHistoryListDTO historyItem = new()
                {
                    Id = ev.Id,
                    Username = ev.User?.Username ?? "N/A",
                    VerifiedEmail = email,
                    FraudScore = fraudScore,
                    ScannedStatus = ev.Status.ToString(),
                    Valid = valid.ToString(),
                    UserStatus = ev.User!.IsDeleted ? "Inactive" : ev.User.IsBlocked ? "Blocked" : "Active"
                };

                historyList.Add(historyItem);
            }

            double averageFraudScore = fraudScoreCount > 0 ? (double)fraudScoreSum / fraudScoreCount : 0;
            double successRate = totalItems > 0 ? (double)successVerifications / totalItems * 100 : 0;

            AdminEmailVerificationDashboardDTO result = new()
            {
                TotalEmailVerifications = totalItems,
                ValidEmails = validCount,
                AverageFraudScore = Math.Round(averageFraudScore, 2),
                SuccessRate = Math.Round(successRate, 2),
                EmailVerificationHistoryList = historyList,
                TotalItems = totalItems,
                EmailVerificationDateTimeStates = ChartData,
                DashboardUsers = users.Select(u => new DashboardUserDTO
                {
                    UserId = u.Id,
                    Username = u.Username,
                    Status = u.IsDeleted ? "Inactive" : u.IsBlocked ? "Blocked" : "Active",
                }).ToList(),
            };
            return result;
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
                int currentInterval = (int)((now - today).TotalHours / 3);
                var intervalCounts = Enumerable.Range(0, currentInterval + 1)
                    .Select(i =>
                    {
                        DateTime intervalStart = today.AddHours(i * 3);
                        DateTime intervalEnd = intervalStart.AddHours(3);
                        string label = $"{intervalStart:hh:mm tt}";
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

            int currentInterval = (int)((now - today).TotalHours / 3);

            List<EmailVerification> todayVerifications = await _dbContext.EmailVerifications
            .Where(u => u.UserId == userId && u.CreatedAt >= today)
            .ToListAsync();


            List<(string label, int count)> intervalCounts = Enumerable.Range(0, currentInterval + 1)
            .Select(i =>
            {
                DateTime intervalStart = today.AddHours(i * 3);
                DateTime intervalEnd = intervalStart.AddHours(3);
                string label = $"{intervalStart:hh:mm tt}";
                int count = todayVerifications.Where(u => u.UserId == userId).Count(ev =>
                    ev.CreatedAt >= intervalStart && ev.CreatedAt < intervalEnd);
                return (label, count);
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

        public async Task<ProxyVpnDetectionDashboardDTO> GetProxyVpnDetectionDashboard(List<int>? userIds)
        {
            List<User> users = await _dbContext.Users.ToListAsync();

            List<ProxyVpnDetection> data = await _dbContext.ProxyVpnDetections
            .Include(u => u.User)
            .Where(u => userIds == null || userIds.Contains(u.UserId))
            .ToListAsync();

            List<ProxyVpnDetection> latestEntries = data
                .GroupBy(u =>
                {
                    JsonDocument json = u.ProxyVpnRequestParam;
                    string? ip = json.RootElement.TryGetProperty("IpAddress", out JsonElement ipProp) ? ipProp.GetString() : "unknown";
                    return new { u.UserId, IpAddress = ip };
                })
                .Select(g => g.OrderByDescending(u => u.CreatedAt).First())
                .ToList();

            List<ProxyVpnDetection> uniqueIPAddresses = latestEntries.GroupBy(u =>
            {
                JsonDocument? json = u.ProxyVpnRequestParam;
                string? ip = json.RootElement.TryGetProperty("IpAddress", out JsonElement ipProp) ? ipProp.GetString() : "unknown";
                return new { IpAddress = ip };
            }).Select(g => g.OrderByDescending(u => u.CreatedAt).First()).ToList();

            int totalIPVerifications = uniqueIPAddresses.Count;
            List<ContinentIPStatsDTO> continentIPStats = uniqueIPAddresses
            .Select(u =>
            {
                JsonDocument? json = u.ProxyVpnResponseParam;
                string timezone = json.RootElement.TryGetProperty("timezone", out JsonElement tzProp) ? tzProp.GetString() ?? "Unknown/Unknown" : "Unknown/Unknown";
                string continent = timezone.Contains('/') ? timezone.Split('/')[0] : "Unknown";
                return continent;
            })
            .GroupBy(continent => continent)
            .Select(g => new ContinentIPStatsDTO
            {
                ContinentName = g.Key,
                Percentage = Math.Round((double)g.Count() / uniqueIPAddresses.Count * 100, 2)
            })
            .ToList();

            int highRiskIPs = uniqueIPAddresses
            .Count(u =>
            {
                JsonDocument? json = u.ProxyVpnResponseParam;
                return json.RootElement.TryGetProperty("fraudScore", out JsonElement score) && score.GetInt32() > 75;
            });

            double averageFraudScore = uniqueIPAddresses.Count != 0 ? uniqueIPAddresses.Average(u =>
            {
                JsonDocument? json = u.ProxyVpnResponseParam;
                return json.RootElement.TryGetProperty("fraudScore", out JsonElement score) ? score.GetInt32() : 0;
            }) : 0;

            int vpnProxyTorCount = uniqueIPAddresses.Count != 0 ? uniqueIPAddresses.Count(u =>
            {
                JsonDocument? json = u.ProxyVpnResponseParam;
                return (json.RootElement.TryGetProperty("vpn", out JsonElement vpn) && vpn.GetBoolean()) ||
                        (json.RootElement.TryGetProperty("proxy", out JsonElement proxy) && proxy.GetBoolean()) ||
                        (json.RootElement.TryGetProperty("tor", out JsonElement tor) && tor.GetBoolean());
            }) : 0;
            double vpnProxyTorUsage = totalIPVerifications > 0 ? ((double)vpnProxyTorCount / totalIPVerifications * 100) : 0;

            return new ProxyVpnDetectionDashboardDTO
            {
                TotalIPVerifications = totalIPVerifications,
                AverageFraudScore = Math.Round(averageFraudScore, 2),
                HighRiskIPs = highRiskIPs,
                VPNProxyTorUsage = Math.Round(vpnProxyTorUsage, 2),
                DashboardUsers = users.Select(u => new DashboardUserDTO
                {
                    UserId = u.Id,
                    Username = u.Username,
                    Status = u.IsDeleted ? "Inactive" : u.IsBlocked ? "Blocked" : "Active",
                }).ToList(),
                ProxyVpnDetectionHistoryList = latestEntries.OrderBy(u => u.Id).Take(5).Select(x =>
                {
                    int fraudScore = x.ProxyVpnResponseParam.RootElement.TryGetProperty("fraudScore", out JsonElement scoreElement)
                    ? scoreElement.GetInt16()
                    : 0;

                    string? riskLevel = fraudScore > 75 ? "High" :
                    fraudScore >= 25 ? "Medium" : "Low";

                    bool isTor = x.ProxyVpnResponseParam.RootElement.TryGetProperty("tor", out JsonElement torElement) && torElement.GetBoolean();
                    bool isVpn = x.ProxyVpnResponseParam.RootElement.TryGetProperty("vpn", out JsonElement vpnElement) && vpnElement.GetBoolean();
                    bool isProxy = x.ProxyVpnResponseParam.RootElement.TryGetProperty("proxy", out var proxyElement) && proxyElement.GetBoolean();

                    string? connectionType = isTor ? "TOR" :
                    isVpn ? "VPN" :
                    isProxy ? "Proxy" :
                    "Normal";
                    return new ProxyVpnDetectionHistoryListDTO
                    {
                        Id = x.Id,
                        Username = x.User!.Username,
                        IpAddress = x.ProxyVpnRequestParam.RootElement.GetProperty("IpAddress").ToString(),
                        RiskStatus = riskLevel,
                        VPNProxyTor = connectionType,
                        Status = x.User.IsDeleted ? "Inactive" : x.User.IsBlocked ? "Blocked" : "Active"

                    };
                }).ToList(),
                CurrentPage = 1,
                PageSize = 5,
                TotalItems = latestEntries.Count,
                ContinentIPStats = continentIPStats
            };
        }

        public async Task<List<EmailVerificationDateTimeStatesDTO>> GetAdminEmailVerificationChart(List<int>? userIds, string range)
        {
            List<int> scans = new();
            List<string> labels = new();
            DateTime now = DateTime.Now;
            DateTime today = DateTime.Today;

            IQueryable<EmailVerification> allScans = _dbContext.EmailVerifications
                .Where(e => userIds!.Contains(e.UserId));

            if (range == "today")
            {
                int currentInterval = (int)((now - today).TotalHours / 3);
                var intervalCounts = Enumerable.Range(0, currentInterval + 1)
                    .Select(i =>
                    {
                        DateTime intervalStart = today.AddHours(i * 3);
                        DateTime intervalEnd = intervalStart.AddHours(3);
                        string label = $"{intervalStart:hh:mm tt}";
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
                    int count = await allScans.CountAsync(ev => ev.CreatedAt >= date && ev.CreatedAt < nextDate);
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
                    int count = await allScans.CountAsync(ev => ev.CreatedAt >= monthStart && ev.CreatedAt < monthEnd);
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
                    int count = await allScans.CountAsync(ev => ev.CreatedAt >= yearStart && ev.CreatedAt < yearEnd);
                    labels.Add(label);
                    scans.Add(count);
                }
            }
            List<EmailVerificationDateTimeStatesDTO> resultDto = new();
            for (int i = 0; i < Math.Min(labels.Count, scans.Count); i++)
            {
                resultDto.Add(new EmailVerificationDateTimeStatesDTO
                {
                    EmailVerificationCount = scans[i],
                    CreatedAt = labels[i]
                });
            }
            return resultDto;
        }

        public async Task<AdminDashboardDTO> GetAdminDashboardDataAsync()
        {
            int totalUsers = await _dbContext.Users.CountAsync();
            int totalEmailVerifications = await _dbContext.EmailVerifications.CountAsync();
            List<ProxyVpnDetection> proxyVpnDetections = await _dbContext.ProxyVpnDetections.ToListAsync();
            int totalProxyVpnDetections = proxyVpnDetections.GroupBy(u =>
            {
                JsonDocument json = u.ProxyVpnRequestParam;
                string? ip = json.RootElement.TryGetProperty("IpAddress", out JsonElement ipProp) ? ipProp.GetString() : "unknown";
                return new { IpAddress = ip };
            }).Count();
            int totalHelpRequests = await _dbContext.HelpAndSupports.CountAsync();
            List<UserRegistrationsChartDTO>? userChart = await GetUserRegistrationsChartAsync("today");
            ServiceUsageChartDTO? serviceUsageChart = await GetServiceUsageChartAsync("today");

            return new AdminDashboardDTO
            {
                TotalUserRegistrations = totalUsers,
                TotalEmailVerifications = totalEmailVerifications,
                TotalIPVerifications = totalProxyVpnDetections,
                TotalHelpDeskRequest = totalHelpRequests,
                UserRegistrationsChart = userChart,
                ServiceUsageChart = serviceUsageChart
            };
        }

        public async Task<List<UserRegistrationsChartDTO>> GetUserRegistrationsChartAsync(string range)
        {
            string normalizedRange = string.IsNullOrWhiteSpace(range) ? "today" : range.Trim().ToLower();
            DateTime now = DateTime.Now;
            DateTime today = now.Date;
            List<string> labels = new();
            List<int> counts = new();
            IQueryable<User> usersQuery = _dbContext.Users;
            if (normalizedRange == "today")
            {
                int currentHour = now.Hour;
                for (int hour = 0; hour <= (currentHour); hour += 3)
                {
                    DateTime start = today.AddHours(hour);
                    DateTime end = start.AddHours(3);
                    string label = $"{start:h tt}-{end:h tt}";

                    int count = await usersQuery.CountAsync(u => u.CreatedAt >= start && u.CreatedAt < end);
                    labels.Add(label);
                    counts.Add(count);
                }
            }
            else if (normalizedRange == "currentmonth")
            {
                DateTime monthStart = new(now.Year, now.Month, 1);
                int daysInMonth = DateTime.DaysInMonth(now.Year, now.Month);

                for (int day = 1; day <= daysInMonth; day++)
                {
                    DateTime dayStart = new(now.Year, now.Month, day);
                    DateTime dayEnd = dayStart.AddDays(1);
                    string label = dayStart.ToString("dd MMM");

                    int count = await usersQuery.CountAsync(u => u.CreatedAt >= dayStart && u.CreatedAt < dayEnd);
                    labels.Add(label);
                    counts.Add(count);
                }
            }
            else if (normalizedRange == "monthly")
            {
                DateTime yearStart = new(now.Year, 1, 1);

                for (int month = 1; month <= 12; month++)
                {
                    DateTime monthStart = new(now.Year, month, 1);
                    DateTime monthEnd = monthStart.AddMonths(1);
                    string label = monthStart.ToString("MMM");

                    int count = await usersQuery.CountAsync(u => u.CreatedAt >= monthStart && u.CreatedAt < monthEnd);
                    labels.Add(label);
                    counts.Add(count);
                }
            }
            else if (normalizedRange == "last5years" || normalizedRange == "yearly")
            {
                int startYear = now.Year - 4;
                for (int year = startYear; year <= now.Year; year++)
                {
                    DateTime yearStart = new(year, 1, 1);
                    DateTime yearEnd = yearStart.AddYears(1);
                    string label = year.ToString();

                    int count = await usersQuery.CountAsync(u => u.CreatedAt >= yearStart && u.CreatedAt < yearEnd);
                    labels.Add(label);
                    counts.Add(count);
                }
            }
            else
            {
                DateTime startDate = now.AddDays(-30);
                for (int i = 0; i <= 30; i++)
                {
                    DateTime dayStart = startDate.AddDays(i);
                    DateTime dayEnd = dayStart.AddDays(1);
                    string label = dayStart.ToString("yyyy-MM-dd");

                    int count = await usersQuery.CountAsync(u => u.CreatedAt >= dayStart && u.CreatedAt < dayEnd);
                    labels.Add(label);
                    counts.Add(count);
                }
            }

            var result = new List<UserRegistrationsChartDTO>();
            for (int i = 0; i < labels.Count; i++)
            {
                result.Add(new UserRegistrationsChartDTO
                {
                    CreatedAt = labels[i],
                    UserCount = counts[i]
                });
            }
            return result;
        }


        public async Task<ServiceUsageChartDTO> GetServiceUsageChartAsync(string range)
        {
            string normalizedRange = string.IsNullOrWhiteSpace(range) ? "today" : range.Trim().ToLower();
            DateTime now = DateTime.Now.Date;
            DateTime currentMonthStart = new(now.Year, now.Month, 1);
            DateTime startDate = normalizedRange == "today" ? now : currentMonthStart;

            int dailyLimit = normalizedRange == "today" ? 35 : 1000;
            int emailCount = await _dbContext.EmailVerifications
                .CountAsync(ev => ev.Status == Status.Success && ev.CreatedAt >= startDate);
            int proxyCount = await _dbContext.ProxyVpnDetections
                .CountAsync(pv => pv.Status == Status.Success && pv.CreatedAt >= startDate);

            return new ServiceUsageChartDTO
            {
                RequestCount = emailCount + proxyCount,
                DailyLimit = dailyLimit
            };
        }
    }
}
