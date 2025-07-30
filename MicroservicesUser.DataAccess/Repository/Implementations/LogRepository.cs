using MicroservicesUser.DataAccess.Data;
using MicroservicesUser.DataAccess.Repository.Interfaces;
using MicroservicesUser.Models.DTO;
using MicroservicesUser.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroservicesUser.DataAccess.Repository.Implementations
{
    public class LogRepository : ILogRepository
    {
        private readonly MicroservicesDbContext _context;
        public LogRepository(MicroservicesDbContext context)
        {
            _context = context;
        }
        public async Task<(List<Log>, int)> GetListAsync(PaginationDTO paginationDTO)
        {
            List<Log> logs = await _context.Logs.ToListAsync();
            if (!string.IsNullOrEmpty(paginationDTO.SearchQuery))
            {
                logs = logs.Where(x => x.MachineName!.ToLower().Contains(paginationDTO.SearchQuery.ToLower()) || x.Message!.ToLower().Contains(paginationDTO.SearchQuery.ToLower())).ToList();
            }
            if (paginationDTO.OrderOfSorting == "asc" && paginationDTO.ColumnNameForSorting == "CreatedAt")
            {
                logs = logs.OrderBy(u => u.RaiseDate).ToList();
            }
            else
            {
                logs = logs.OrderByDescending(u => u.RaiseDate).ToList();
            }
            if (paginationDTO.ColumnNameForFilter == "Status" && !string.IsNullOrEmpty(paginationDTO.FilterValue))
            {
                logs = logs.Where(u => u.Level == Convert.ToInt16(paginationDTO.FilterValue)).ToList();
            }
            int totalCount = logs.Count;
            logs = logs.Skip((paginationDTO.CurrentPage - 1) * paginationDTO.PageSize).Take(paginationDTO.PageSize).ToList();
            return (logs, totalCount);
        }

        public async Task<List<string>> GetSuggestionMessagesList(string searchQuery)
        {
            List<string?> logs = await _context.Logs.Select(log => log.Message).ToListAsync();
            var rankedMessages = logs
                .Select(message => new
                {
                    Message = message,
                    Score = message!.Equals(searchQuery, StringComparison.OrdinalIgnoreCase) ? 100 : message.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ? 50 : 0
                })
                .OrderByDescending(x => x.Score)
                .Select(x => x.Message)
                .Take(5)
                .ToList();

            return rankedMessages!;
        }
    }
}