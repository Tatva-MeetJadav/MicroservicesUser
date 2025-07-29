using MicroservicesUser.Models.ViewModels.History;

namespace MicroservicesUser.BusinessLogic.Interfaces
{
    public interface IExcelExportServices
    {
        byte[] GenerateEmailVerificationExcel(EmailVerificationDetailVM detailVM);
        byte[] GenerateEmailVerificationHistoryListExcel(EmailVerificationListHistoryVM historyListVM);

    }
}
