using BienenstockCorpAPI.Models.AutenticationModels;
using BienenstockCorpAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BienenstockCorpAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthenticationController : ControllerBase 
    {
        #region Constructor
        private readonly AuthenticationService _authenticationService;

        public AuthenticationController(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        #endregion

        #region Endpoints
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest rq)
        {
            var rsp = await _authenticationService.Login(rq);

            var loginResponse = new LoginResponse
            {
                UserId = rsp.UserId,
                Avatar = rsp.Avatar,
                Email = rsp.Email,
                FullName = rsp.FullName,
                UserType = rsp.UserType,
                Success = rsp.Success,
                Message = rsp.Message,
            };

            if (rsp.Success)
            {
                // Set HttpOnly Cookie
                HttpContext.Response.Cookies.Append("bienenstockCorp_token", rsp.Token,
                    new CookieOptions
                    {
                        Expires = rsp.Expiration,
                        HttpOnly = true,
                        Secure = true,
                        IsEssential = true,
                        SameSite = SameSiteMode.None,
                    });

                return Ok(loginResponse);
            }
            else
                return Unauthorized(loginResponse);
        }

        [HttpGet]
        public async Task<IActionResult> GetLoggedUser()
        {
            var rsp = await _authenticationService.GetLoggedUser(new GetLoggedUserRequest { Identity = HttpContext.User.Identity as ClaimsIdentity });

            if (rsp.Success)
                return Ok(rsp);
            else
                return Unauthorized(rsp);
        }
        #endregion
    }
}

