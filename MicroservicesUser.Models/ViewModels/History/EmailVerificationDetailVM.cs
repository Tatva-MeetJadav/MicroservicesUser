using MicroservicesUser.Models.ViewModels.EmailVerification;

namespace MicroservicesUser.Models.ViewModels.History
{
    public class EmailVerificationDetailVM
    {
        public EmailVerificationResponseVM? ResponseVM { get; set; }
        public EmailVerificationRequestVM? RequestVM { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}