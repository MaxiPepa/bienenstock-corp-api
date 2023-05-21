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

            if (rsp.Success)
                return Ok(rsp);
            else
                return Unauthorized(rsp);
        }

        [HttpPost]
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

