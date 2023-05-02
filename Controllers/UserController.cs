using BienenstockCorpAPI.Models.UserModels;
using BienenstockCorpAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BienenstockCorpAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _userService.GetUsers());
        }

        [HttpPost("SaveUser")]
        public async Task<IActionResult> SaveUser([FromBody] SaveUserRequest rq)
        {
            var rsp = await _userService.SaveUser(rq);
                
            if (!rsp.Error)
                return Ok(rsp);
            else
                return BadRequest(rsp);
        }
        #endregion
    }
}
