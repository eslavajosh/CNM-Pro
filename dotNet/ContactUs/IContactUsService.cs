using Sabio.Models.Requests.ContactUs;
using System.Threading.Tasks;

namespace Sabio.Services
{
    public interface IContactUsService
    {
        Task ContactUsEmail(ContactUsAddRequest model);
    }
}