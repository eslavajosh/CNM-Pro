using Sabio.Models.Requests.Email;
using SendGrid;
using System.Threading.Tasks;

namespace Sabio.Services.Interfaces
{
    public interface IEmailServices
    {
        Task SendContactUsEmail(EmailAddRequest model);
        Task SendConfirmEmail(EmailAddRequest model);
        public void confirmEmail(string email, string token);
        Task SendResetPasswordEmail(string email, string token);    }
}