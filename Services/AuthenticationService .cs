using BienenstockCorpAPI.Data;
using BienenstockCorpAPI.Helpers;
using Microsoft.EntityFrameworkCore;
using BienenstockCorpAPI.Models.AutenticationModels;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BienenstockCorpAPI.Services
{
    public class AuthenticationService
    {
        #region Constructor
        private readonly BienenstockCorpContext _context;
        private readonly IConfiguration _configuration;

        public AuthenticationService(BienenstockCorpContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        #endregion

        #region Autentication
        public async Task<TokenResponse> Login(LoginRequest rq)
        {
            if (rq == null)
                return new TokenResponse
                {
                    Success = false,
                    Message = "Invalid request",
                };

            var user = await _context.User
                .Where(x => x.Email == rq.Email && x.PassHash == EncryptionHelper.EncryptSHA256(rq.Password))
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return new TokenResponse
                {
                    Success = false,
                    Message = "Incorrect email or password",
                };
            };

            if (user.Inactive)
            {
                return new TokenResponse
                {
                    Success = false,
                    Message = "This User has been deleted or inactivated",
                };
            };

            var jwt = _configuration.GetSection("Jwt").Get<Jwt>();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),

                new Claim("UserId", user.UserId.ToString()),
                new Claim("Name", user.Name),
                new Claim("LastName", user.LastName),
                new Claim("Email", user.Email),
                new Claim("UserType", user.UserType),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));

            var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    jwt.Issuer,
                    jwt.Audience,
                    claims,
                    expires: DateTime.Now.AddHours(12),
                    signingCredentials: singIn
                );

            return new TokenResponse
            {
                Success = true,
                Message = "Successfully signed in",
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                UserId = user.UserId,
                Avatar = user.Avatar,
                Email = user.Email,
                FullName = user.FullName,
                UserType = user.UserType,
                Expiration = token.ValidTo,
            };
        }

        public async Task<GetLoggedUserResponse> GetLoggedUser(GetLoggedUserRequest rq)
        {
            var token = rq.Identity.TokenVerifier();

            if (!token.Success)
            {
                return new GetLoggedUserResponse
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
                return new GetLoggedUserResponse
                {
                    Success = false,
                    Message = "User not found",
                };
            }

            return new GetLoggedUserResponse
            {
                Success = true,
                Message = "Succesfully retrieved user",
                UserId  = user.UserId,
                Avatar = user.Avatar,
                Email = user.Email,
                FullName = user.FullName,
                UserType = user.UserType,
            };
        }
        #endregion
    }
}
