using MicroservicesUser.DataAccess.Data;
using MicroservicesUser.DataAccess.Repository.Interfaces;
using MicroservicesUser.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroservicesUser.DataAccess.Repository.Implementations
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly MicroservicesUserDbContext _context;
        public NotificationRepository(MicroservicesUserDbContext microservicesUserDbContext)
        {
            _context = microservicesUserDbContext;
        }

        public async Task AddAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public Task<List<Notification>> GetUnreadNotificationList()
        {
            return _context.Notifications.Where(u => u.IsRead == false).Include(u => u.User).Include(u => u.HelpAndSupport).OrderByDescending(u => u.CreatedAt).ToListAsync();
        }

        public async Task UpdateListAsync(List<Notification> notifications)
        {
            _context.Notifications.UpdateRange(notifications);
            await _context.SaveChangesAsync();
        }
    }
}