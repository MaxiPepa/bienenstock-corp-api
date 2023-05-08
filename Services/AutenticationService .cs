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
    public class AutenticationService
    {
        #region Constructor
        private readonly BienenstockCorpContext _context;
        private readonly IConfiguration _configuration;

        public AutenticationService(BienenstockCorpContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        #endregion

        #region Autentication
        public async Task<LoginResponse> Login(LoginRequest rq)
        {
            if (rq == null)
                return new LoginResponse
                {
                    Success = false,
                    Message = "Invalid request",
                };

            var user = await _context.User
                .Where(x => x.Email == rq.Email && x.PassHash == EncryptionHelper.EncryptSHA256(rq.Password))
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return new LoginResponse 
                {
                    Success = false,
                    Message = "Incorrect credentials",
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

            return new LoginResponse
            {
                Success = true,
                Message = "Successfully signed in",
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Avatar = user.Avatar,
                Email = user.Email,
                Fullname = user.Name + " " + user.LastName,
                Expiration = token.ValidTo,
            };
        }
        #endregion
    }
}
