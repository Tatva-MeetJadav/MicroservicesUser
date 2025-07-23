using MicroservicesUser.DataAccess.Data;
using MicroservicesUser.DataAccess.Repository.Interfaces;
using MicroservicesUser.Models.DTO;
using MicroservicesUser.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroservicesUser.DataAccess.Repository.Implementations
{
    public class HelpAndSupportRepository : IHelpAndSupportRepository
    {
        private readonly MicroservicesUserDbContext _context;
        public HelpAndSupportRepository(MicroservicesUserDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(HelpAndSupport helpAndSupport)
        {
            await _context.HelpAndSupports.AddAsync(helpAndSupport);
            await _context.SaveChangesAsync();
        }

        public async Task<(List<HelpAndSupport>, int)> GetListAsync(HelpAndSupportRequestDTO requestDTO)
        {
            IQueryable<HelpAndSupport> helpAndSupports = _context.HelpAndSupports.Include(u => u.User).AsQueryable();
            if (!string.IsNullOrEmpty(requestDTO.PaginationDTO!.SearchQuery))
            {
                helpAndSupports = helpAndSupports.Where(h => h.Subject!.ToLower().Contains(requestDTO.PaginationDTO!.SearchQuery.ToLower()) || h.Message!.ToLower().Contains(requestDTO.PaginationDTO!.SearchQuery.ToLower()) || h.User!.Username!.ToLower().Contains(requestDTO.PaginationDTO!.SearchQuery.ToLower()));
            }

            if (!string.IsNullOrEmpty(requestDTO.CategoryType))
            {
                helpAndSupports = helpAndSupports.Where(h => h.Category! == requestDTO.CategoryType);
            }

            helpAndSupports = helpAndSupports.Where(h => h.CreatedAt >= requestDTO.FromDate.ToDateTime(TimeOnly.MinValue) && h.CreatedAt <= requestDTO.ToDate.ToDateTime(TimeOnly.MaxValue));
            int totalCount = helpAndSupports.Count();
            helpAndSupports = helpAndSupports.OrderByDescending(u => u.CreatedAt);
            helpAndSupports = helpAndSupports.Skip((requestDTO.PaginationDTO.CurrentPage - 1) * requestDTO.PaginationDTO.PageSize).Take(requestDTO.PaginationDTO.PageSize);
            return (await helpAndSupports.ToListAsync(), totalCount);
        }
    }
}
