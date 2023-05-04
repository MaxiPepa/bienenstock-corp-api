using BienenstockCorpAPI.Models.AutenticationModels;
using BienenstockCorpAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BienenstockCorpAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AutenticationController : ControllerBase 
    {
        #region Constructor
        private readonly AutenticationService _autenticationService;

        public AutenticationController(AutenticationService autenticationService)
        {
            _autenticationService = autenticationService;
        }
        #endregion

        #region Endpoints
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest rq)
        {
            var rsp = await _autenticationService.Login(rq);

            if (rsp.Success)
                return Ok(rsp);
            else
                return BadRequest(rsp);
        }
        #endregion
    }
}

