using MicroservicesUser.Models.DTO;
using MicroservicesUser.Models.Models;

namespace MicroservicesUser.DataAccess.Repository.Interfaces
{
    public interface IEmailVerificationRepository
    {
        Task AddAsync(EmailVerification emailVerification);
        Task<(List<EmailVerification>, int)> GetListByUserId(int userId, PaginationDTO paginationVM);
        Task<EmailVerification> GetAsync(int id);
        Task<AdminEmailVerificationDashboardDTO> GetEmailVerificationHistoryList(AdminEmailVerificationHistoryRequestDTO requestDTO);
    }
}