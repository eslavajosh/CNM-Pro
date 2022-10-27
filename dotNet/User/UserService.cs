using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.User;
using Sabio.Models.Requests.Email;
using Sabio.Models.Requests.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sabio.Services
{
    public class UserService : IUserService
    {
        private IAuthenticationService<int> _authenticationService;
        private IDataProvider _dataProvider;

        public UserService(IAuthenticationService<int> authSerice, IDataProvider dataProvider)
        {
            _authenticationService = authSerice;
            _dataProvider = dataProvider;
        }

        public async Task<bool> LogInAsync(string email, string password)
        {
            bool isSuccessful = false;

            IUserAuthData response = Get(email, password);

            if (response != null)
            {
                await _authenticationService.LogInAsync(response);
                isSuccessful = true;
            }

            return isSuccessful;
        }

        public async Task<bool> LogInTest(string email, string password, int id, string[] roles = null)
        {
            bool isSuccessful = false;
            var testRoles = new[] { "User", "Super", "Content Manager" };

            var allRoles = roles == null ? testRoles : testRoles.Concat(roles);

            IUserAuthData response = new UserBase
            {
                Id = id
                ,
                Name = email
                ,
                Roles = allRoles
                ,
                TenantId = "CnmPro"
            };

            Claim fullName = new Claim("CustomClaim", "Sabio Bootcamp");
            await _authenticationService.LogInAsync(response, new Claim[] { fullName });

            return isSuccessful;
        }

        public int Create(UserAddRequest userModel)
        {
            int userId = 0;
            string password = userModel.Password;
            string salt = BCrypt.BCryptHelper.GenerateSalt();
            string hashedPassword = BCrypt.BCryptHelper.HashPassword(password, salt);

            string procName = "[dbo].[Users_InsertV2]";

            _dataProvider.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection paramCol)
                {
                    paramCol.AddWithValue("@Email", userModel.Email);
                    paramCol.AddWithValue("@Password", hashedPassword);
                    paramCol.AddWithValue("@IsConfirmed", false);
                    paramCol.AddWithValue("@UserStatusId", 1);
                    paramCol.AddWithValue("@RoleId", 1);

                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                    idOut.Direction = ParameterDirection.Output;

                    paramCol.Add(idOut);
                },
                returnParameters: delegate (SqlParameterCollection returnCol)
                {
                    object oId = returnCol["@Id"].Value;

                    Int32.TryParse(oId.ToString(), out userId);
                });

            return userId;
        }

        public void UpdatePassword(UserUpdatePasswordRequest model)
        {
            string password = model.Password;
            string salt = BCrypt.BCryptHelper.GenerateSalt();
            string hashedPassword = BCrypt.BCryptHelper.HashPassword(password, salt); //hash password here

            string procName = "[dbo].[Users_Update]";

            _dataProvider.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", model.Id);
                    col.AddWithValue("@Email", model.Email);
                    col.AddWithValue("@Password", hashedPassword);
                    col.AddWithValue("@IsConfirmed", true);
                    col.AddWithValue("@UserStatusId", 1);

                },
                returnParameters: null);
        }

        public void ConfirmUser(string token)
        {
            string procName = "[dbo].[Users_UpdateIsConfirm]";

            _dataProvider.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection paramCol)
                {
                    paramCol.AddWithValue("@Token", token);
                },
                returnParameters: null
                );
        }

        public void InsertToken(string token, int userId, int tokenType)
        {

            string procName = "[dbo].[UserTokens_Insert]";

            _dataProvider.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection paramCol)
                {
                    paramCol.AddWithValue("@Token", token);
                    paramCol.AddWithValue("@UserId", userId);
                    paramCol.AddWithValue("@TokenType", tokenType);
                },
                returnParameters: null);
        }


        public bool VerifyEmail(string email)
        {
            string procName = "[dbo].[Users_Select_ByEmail]";
            bool isUser = false;

            User user = new User();
            _dataProvider.ExecuteCmd(procName, delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@email", email);
            },
            delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                user.Id = reader.GetInt32(startingIndex++);
                user.Email = reader.GetSafeString(startingIndex++);

                if (!string.IsNullOrEmpty(email) && user.Email == email)
                {
                    isUser = true;
                }

            });
             return isUser;
        }

        public int GetByEmail(string email)
        {
            string procName = "[dbo].[Users_Select_AuthDataV2]";
            string passwordFromDb = "";

            UserBase user = null;
            _dataProvider.ExecuteCmd(procName, delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Email", email);
            },
            singleRecordMapper: delegate (IDataReader reader, short set)
            {
               UserBase aUser = new UserBase();
                aUser.Id = reader.GetSafeInt32(0);
                aUser.Name = reader.GetSafeString(1);
                passwordFromDb = reader.GetSafeString(2);
                aUser.Roles = new[] { reader.GetSafeString(3) };
                aUser.TenantId = "CnmPro";
                user = aUser;
            });
            return user.Id;
        }

        private IUserAuthData Get(string email, string password)
        {
            string passwordFromDb = "";
            UserBase user = null;

            string procName = "[dbo].[Users_Select_AuthDataV2]";

            _dataProvider.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection paramCol)
                {
                    paramCol.AddWithValue("@Email", email);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    UserBase aUser = new UserBase();
                    aUser.Id = reader.GetSafeInt32(0);
                    aUser.Name = reader.GetSafeString(1);
                    passwordFromDb = reader.GetSafeString(2);
                    aUser.Roles = new[] {reader.GetSafeString(3)};
                    aUser.TenantId = "CnmPro";
 
                    bool isValidCredentials = BCrypt.BCryptHelper.CheckPassword(password, passwordFromDb);

                    if (isValidCredentials)
                    {
                        user = aUser;
                    }
                }
                );
     
            return user;
        }
        private static User MapSingleUser(IDataReader reader)
        {
            User user = new User();
            int index = 0;
            user.Id = reader.GetSafeInt32(index++);
            user.Email = reader.GetSafeString(index++);
            user.Roles = new[] {reader.GetSafeString(index++)};
            user.TenantId = "CnmPro";

            return user;
        }

        public void DeleteToken(string token)
        {
            string procName = "[dbo].[UserTokens_Delete_ByToken]";

            _dataProvider.ExecuteNonQuery(procName, delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Token", token);
            });
        }

    }
}