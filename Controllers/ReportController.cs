using BienenstockCorpAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BienenstockCorpAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Authorize]
    public class ReportController : ControllerBase
    {
        #region Constructor
        private readonly ReportService _reportService;

        public ReportController(ReportService reportService)
        {
            _reportService = reportService;
        }
        #endregion

        #region Endpoints
        [HttpGet]
        public async Task<IActionResult> GetCompanyStats()
        {
            return Ok(await _reportService.GetCompanyStats(HttpContext.User.Identity as ClaimsIdentity));
        }

        [HttpGet]
        public async Task<IActionResult> GetStatistics()
        {
            var rsp = await _reportService.GetStatistics(HttpContext.User.Identity as ClaimsIdentity);

            if (rsp.Success)
                return Ok(rsp);
            else
                return BadRequest(rsp);
        }
        #endregion
    }
}
