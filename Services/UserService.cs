using BienenstockCorpAPI.Data.Entities;
using BienenstockCorpAPI.Data;
using BienenstockCorpAPI.Models.UserModels;
using BienenstockCorpAPI.Helpers;
using BienenstockCorpAPI.Helpers.Consts;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Security.Claims;

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
                    FullName = u.FullName,
                    Email = u.Email,
                    UserType = u.UserType,
                }).OrderBy(x => x.FullName).ToList(),
            };
        }

        public async Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest rq, ClaimsIdentity? identity)
        {
            var token = identity.TokenVerifier();
            var passwordRegx = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\w\s]).{6,}$");
            var validation = ValidateChangePassword(rq,token);

            if (!string.IsNullOrEmpty(validation))
            {
                return new ChangePasswordResponse
                {
                    Success = false,
                    Message = validation,
                };
            }
           
            var user = await _context.User
                .Where(x => x.UserId == token.UserId)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return new ChangePasswordResponse
                {
                    Success = false,
                    Message = "User not found",
                };
            }

            if (user.PassHash != EncryptionHelper.EncryptSHA256(rq.Password))
            {
                return new ChangePasswordResponse
                {
                    Success = false,
                    Message = "Incorrect password",
                };
            }

            user.PassHash = EncryptionHelper.EncryptSHA256(rq.NewPassword);

            try
            {
                await _context.SaveChangesAsync();
                return new ChangePasswordResponse
                {
                    Success = true,
                    Message = "Succesfully changed your Password"
                };
            }
            catch (Exception ex)
            {
                return new ChangePasswordResponse
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }
        
        public async Task<SaveChangeAvatarResponse> ChangeAvatar(SaveChangeAvatarRequest rq, ClaimsIdentity? identity)
        {
            var token = identity.TokenVerifier();

            if (!token.Success)
            {
                return new SaveChangeAvatarResponse
                {
                    Success = false,
                    Message = token.Message,
                };
            }

            var user = await _context.User
                .Where(x => x.UserId == token.UserId)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return new SaveChangeAvatarResponse
                {
                    Success = false,
                    Message = "User not found",
                };
            }

            user.Avatar = rq.Avatar;

            try
            {
                await _context.SaveChangesAsync();
                return new SaveChangeAvatarResponse 
                {
                    Success = true, 
                    Avatar = rq.Avatar,
                    Message = "Succesfully changed Avatar" 
                };
            }
            catch (Exception ex)
            {
                return new SaveChangeAvatarResponse
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<ChangeEmailResponse> ChangeEmail(ChangeEmailRequest rq, ClaimsIdentity? identity)
        {
            var token = identity.TokenVerifier();
            var emailRegx = new Regex(@"\S+@\S+\.\S+");

            if (!token.Success)
            {
                return new ChangeEmailResponse
                {
                    Success = false,
                    Message = token.Message,
                };
            }

            if (string.IsNullOrEmpty(rq.Email) || !emailRegx.IsMatch(rq.Email))
            {
                return new ChangeEmailResponse
                {
                    Success = false,
                    Message = "Provide a valid Email",
                };
            }

            var user = await _context.User
                .Where(x => x.UserId == token.UserId)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return new ChangeEmailResponse
                {
                    Success = false,
                    Message = "User not found",
                };
            }

            user.Email = rq.Email;

            try
            {
                await _context.SaveChangesAsync();
                return new ChangeEmailResponse
                {
                    Success = true,
                    Email = rq.Email,
                    Message = "Succesfully changed your Email"
                };
            }
            catch (Exception ex)
            {
                return new ChangeEmailResponse
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<SaveUserResponse> SaveUser(SaveUserRequest rq)
        {
            var validation = ValidateSaveUser(rq);

            if (validation != String.Empty)
            {
                return new SaveUserResponse
                {
                    Message = validation,
                    Success = false,
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
                    FullName = user.FullName,
                    Email = user.Email,
                    Message = "User successfully created",
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                return new SaveUserResponse
                {
                    Message = ex.Message,
                    Success = false,
                };
            }
        }

        public async Task<ModifyUserResponse> ModifyUser(ModifyUserRequest rq, ClaimsIdentity? identity)
        {
            var token = identity.TokenVerifier();

            var validation = ValidateModifyUser(rq,token);

            if (validation != String.Empty)
            {
                return new ModifyUserResponse
                {
                    Message = validation,
                    Success = false,
                };
            }

            var user = _context.User
                .FirstOrDefault(r => r.UserId == rq.Id);

            if (user == null)
            {
                return new ModifyUserResponse
                {
                    Message = "User not found",
                    Success = false,
                };
            }

            user.Name = rq.Name;
            user.LastName = rq.LastName;
            user.Email = rq.Email;
            user.UserType = rq.UserType;

            try
            {
                await _context.SaveChangesAsync();

                return new ModifyUserResponse
                {
                    Message = "User successfully modified",
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                return new ModifyUserResponse
                {
                    Message = ex.Message,
                    Success = false,
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

        private static string ValidateChangePassword(ChangePasswordRequest rq, TokenVerifyResponse token)
        {
            var error = String.Empty;
            var passwordRegx = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\w\s]).{6,}$");

            if (rq == null)
            {
                error = "Invalid Request";
            }
            else if (!token.Success)
            {
                error = token.Message;
            }
            else if (string.IsNullOrEmpty(rq.NewPassword) || !passwordRegx.IsMatch(rq.NewPassword))
            {
                error = "Invalid new password";
            }
            else if (rq.NewPassword != rq.ConfirmPassword)
            {
                error = "The passwords don't match";
            }

            return error;
        }

        private static string ValidateModifyUser(ModifyUserRequest rq,TokenVerifyResponse token)
        {
            var error = String.Empty;
            var emailRegx = new Regex(@"\S+@\S+\.\S+");

            if (rq == null)
            {
                error = "Invalid Request";
            }
            else if (!token.Success || token.UserType != UserType.ADMIN)
            {
                error = "Insufficient permissions";
            }
            else if (string.IsNullOrEmpty(rq.Name) || string.IsNullOrEmpty(rq.LastName))
            {
                error = "Invalid name or last name";
            }
            else if (string.IsNullOrEmpty(rq.Email) || !emailRegx.IsMatch(rq.Email))
            {
                error = "Provide a valid Email";
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
