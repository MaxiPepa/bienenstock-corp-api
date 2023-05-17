using BienenstockCorpAPI.Models.UserModels;
using BienenstockCorpAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BienenstockCorpAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Authorize]
    public class LogController : ControllerBase
    {
        #region Constructor
        private readonly LogService _logService;

        public LogController(LogService logService)
        {
            _logService = logService;
        }
        #endregion

        #region Endpoints
        [HttpGet]
        public async Task<IActionResult> GetLogs()
        {
            return Ok(await _logService.GetLogs());
        }
        #endregion
    }
}
