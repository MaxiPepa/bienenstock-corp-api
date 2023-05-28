using BienenstockCorpAPI.Models.SaleModels;
using BienenstockCorpAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BienenstockCorpAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Authorize]
    public class SaleController : ControllerBase
    {
        #region Constructor
        private readonly SaleService _saleService;

        public SaleController(SaleService saleService)
        {
            _saleService = saleService;
        }
        #endregion

        #region Endpoints
        [HttpGet]
        public async Task<IActionResult> GetSales()
        {
            var rsp = await _saleService.GetSales(HttpContext.User.Identity as ClaimsIdentity);

            if (rsp.Success)
                return Ok(rsp);
            else
                return BadRequest(rsp);
        }

        [HttpPost]
        public async Task<IActionResult> SaveSale([FromBody] SaveSaleRequest rq)
        {
            var rsp = await _saleService.SaveSale(rq, HttpContext.User.Identity as ClaimsIdentity);

            if (rsp.Success)
                return Ok(rsp);
            else
                return BadRequest(rsp);
        }

        [HttpPost]
        public async Task<IActionResult> DispatchSale([FromBody] DispatchSaleRequest rq)
        {
            var rsp = await _saleService.DispatchSale(rq, HttpContext.User.Identity as ClaimsIdentity);

            if (rsp.Success)
                return Ok(rsp);
            else
                return BadRequest(rsp);
        }
        #endregion
    }
}
