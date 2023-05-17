using BienenstockCorpAPI.Models.UserModels;
using BienenstockCorpAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BienenstockCorpAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        #region Constructor
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }
        #endregion

        #region Endpoints
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _userService.GetUsers());
        }

        [HttpPost]
        public async Task<IActionResult> SaveUser([FromBody] SaveUserRequest rq)
        {
            var rsp = await _userService.SaveUser(rq);

            if (rsp.Success)
                return Ok(rsp);
            else
                return BadRequest(rsp);
        }
        #endregion
    }
}
