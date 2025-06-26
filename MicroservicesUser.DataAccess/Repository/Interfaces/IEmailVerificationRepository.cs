using MicroservicesUser.Models.Models;
using MicroservicesUser.Models.ViewModels;

namespace MicroservicesUser.DataAccess.Repository.Interfaces
{
    public interface IEmailVerificationRepository
    {
        Task AddAsync(EmailVerification emailVerification);
        Task<(List<EmailVerification>, int)> GetListByUserId(int userId, PaginationVM paginationVM);
        Task<EmailVerification> GetAsync(int id);
    }
}