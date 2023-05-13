using BienenstockCorpAPI.Models.Message;
using BienenstockCorpAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BienenstockCorpAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        [HttpGet("GetMessages")]
        public async Task<IActionResult> GetMessages()
        {
            return Ok(await _messageService.GetMessages());
        }

        [HttpPost("SaveMessage")]
        public async Task<IActionResult> SaveMessage([FromBody] SaveMessageRequest rq)
        {
            var rsp = await _messageService.SaveMessage(rq);

            if (rsp.Success)
                return Ok(rsp);
            else
                return BadRequest(rsp);   
        }
        #endregion
    }
}
