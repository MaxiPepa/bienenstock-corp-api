﻿using BienenstockCorpAPI.Models.UserModels;
using BienenstockCorpAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BienenstockCorpAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _userService.GetUsers());
        }

        [HttpPost("SaveUser")]
        public async Task<IActionResult> SaveUser([FromBody] SaveUserRequest rq)
        {
            var rsp = await _userService.SaveUser(rq);

            if (rsp.Success)
                return Ok(rsp);
            else
                return BadRequest(rsp);
        }

        [HttpPost("ChangeAvatar")]
        public async Task<IActionResult> ChangeAvatar([FromBody] SaveChangeAvatarRequest rq)
        {
            var rsp = await _userService.ChangeAvatar(rq, HttpContext.User.Identity as ClaimsIdentity);

            if (rsp.Success)
                return Ok(rsp);
            else
                return Unauthorized(rsp);
        }

        [HttpPost("ChangeEmail")]
        public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailRequest rq)
        {
            var rsp = await _userService.ChangeEmail(rq, HttpContext.User.Identity as ClaimsIdentity);

            if (rsp.Success)
                return Ok(rsp);
            else
                return Unauthorized(rsp);
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest rq)
        {
            var rsp = await _userService.ChangePassword(rq, HttpContext.User.Identity as ClaimsIdentity);

            if (rsp.Success)
                return Ok(rsp);
            else
                return Unauthorized(rsp);
        }
        #endregion
    }
}
