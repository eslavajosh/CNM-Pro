using Sabio.Models.Domain;
using Sabio.Models.Domain.User;
using Sabio.Models.Requests.Email;
using Sabio.Models.Requests.User;
using System.Threading.Tasks;

namespace Sabio.Services
{
    public interface IUserService
    {
        int Create(UserAddRequest userModel);

        void InsertToken(string token, int userId, int tokenType);

        void ConfirmUser(string token);

        Task<bool> LogInAsync(string email, string password);

        Task<bool> LogInTest(string email, string password, int id, string[] roles = null);

        bool VerifyEmail(string email);

        int GetByEmail(string email);

        void UpdatePassword(UserUpdatePasswordRequest model);

        void DeleteToken(string token);
    }
}