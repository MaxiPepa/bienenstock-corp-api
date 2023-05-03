using BienenstockCorpAPI.Data.Entities;
using BienenstockCorpAPI.Data;
using BienenstockCorpAPI.Models.UserModels;
using BienenstockCorpAPI.Helpers;
using BienenstockCorpAPI.Helpers.Consts;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace BienenstockCorpAPI.Services
{
    public class UserService
    {
        #region Constructor
        private readonly BienenstockCorpContext _context;

        public UserService(BienenstockCorpContext context)
        {
            _context = context;
        }
        #endregion


        #region User
        public async Task<GetUsersResponse> GetUsers()
        {
            var users = await _context.User
                .ToListAsync();


            return new GetUsersResponse
            {
                Users = users.Select(u => new GetUsersResponse.Item
                {
                    UserId = u.UserId,
                    FullName = u.Name + " " + u.LastName,
                    Email = u.Email,
                    UserType = u.UserType,
                }).OrderBy(x => x.FullName).ToList(),
            };
        }

        public async Task<SaveUserResponse> SaveUser(SaveUserRequest rq)
        {
            var validation = ValidateSaveUser(rq);

            if (validation != String.Empty)
            {
                return new SaveUserResponse
                {
                    Message = validation,
                    Error = true,
                };  
            }

            var user = new User
            {
                Name = rq.Name,
                LastName = rq.LastName,
                Email = rq.Email,
                PassHash = EncryptionHelper.EncryptSHA256(rq.Password),
                UserType = rq.UserType,
            };

            try
            {
                _context.User.Add(user);
                await _context.SaveChangesAsync();

                return new SaveUserResponse
                {
                    FullName = user.Name + " " + user.LastName,
                    Email = user.Email,
                    Message = "User successfully created",
                    Error = false,
                };
            }
            catch (Exception ex)
            {
                return new SaveUserResponse
                {
                    Message = ex.Message,
                    Error = true,
                };
            }
        }
        #endregion

        #region Validations
        private static string ValidateSaveUser(SaveUserRequest rq)
        {
            var error = String.Empty;
            var emailRegx = new Regex(@"\S+@\S+\.\S+");
            var passwordRegx = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\w\s]).{6,}$");

            if (rq == null)
            {
                error = "Invalid Request";
            }
            else if (string.IsNullOrEmpty(rq.Name) || string.IsNullOrEmpty(rq.LastName))
            {
                error = "Invalid name or last name";
            }
            else if (string.IsNullOrEmpty(rq.Email) || !emailRegx.IsMatch(rq.Email))
            {
                error = "Provide a valid Email";
            }
            else if (string.IsNullOrEmpty(rq.Password) || !passwordRegx.IsMatch(rq.Password))
            {
                error = "Provide a valid password (Longer than 6 characters, one uppercase, one lowercase, one special character and one number)";
            }
            else if (string.IsNullOrEmpty(rq.UserType) ||
                rq.UserType != UserType.ADMIN &&
                rq.UserType != UserType.BUYER &&
                rq.UserType != UserType.SELLER &&
                rq.UserType != UserType.DEPOSITOR &&
                rq.UserType != UserType.ANALYST) 
            {
                error = "Invalid user type";
            }

            return error;
        }
        #endregion
    }
}
