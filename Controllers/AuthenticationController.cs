using BienenstockCorpAPI.Models.AutenticationModels;
using BienenstockCorpAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BienenstockCorpAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest rq)
        {
            var rsp = await _authenticationService.Login(rq);

            if (rsp.Success)
                return Ok(rsp);
            else
                return BadRequest(rsp);
        }

        [HttpPost("GetLoggedUser")]
        public async Task<IActionResult> GetLoggedUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            var rsp = await _authenticationService.GetLoggedUser(identity);

            if (rsp.Success)
                return Ok(rsp);
            else
                return BadRequest(rsp);
        }
        #endregion
    }
}

