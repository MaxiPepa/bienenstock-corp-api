﻿using BienenstockCorpAPI.Models.MessageModels;
using BienenstockCorpAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BienenstockCorpAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Authorize]
    public class MessageController : ControllerBase
    {
        #region Constructor
        private readonly MessageService _messageService;

        public MessageController(MessageService messageService)
        {
            _messageService = messageService;
        }
        #endregion

        #region Endpoints
        [HttpGet]
        public async Task<IActionResult> GetMessages()
        {
            return Ok(await _messageService.GetMessages());
        }

        [HttpPost]
        public async Task<IActionResult> SaveMessage([FromBody] SaveMessageRequest rq)
        {
            var rsp = await _messageService.SaveMessage(rq, HttpContext.User.Identity as ClaimsIdentity);

            if (rsp.Success)
                return Ok(rsp);
            else
                return BadRequest(rsp);   
        }
        #endregion
    }
}
