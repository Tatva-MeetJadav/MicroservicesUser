using MicroservicesUser.Models.Models;

namespace MicroservicesUser.DataAccess.Repository.Interfaces
{
    public interface IEmailVerificationRepository
    {
        Task AddAsync(EmailVerification emailVerification);
        Task<(List<EmailVerification>, int)> GetListByUserId(int userId, int page, int pageSize, string searchQuery);
    }
}