using BienenstockCorpAPI.Models.PurchaseModels;
using BienenstockCorpAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BienenstockCorpAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Authorize]
    public class PurchaseController : ControllerBase
    {
        #region Constructor
        private readonly PurchaseService _purchaseService;

        public PurchaseController(PurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }
        #endregion

        #region Endpoints
        [HttpGet]
        public async Task<IActionResult> GetPurchases()
        {
            var rsp = await _purchaseService.GetPurchases(HttpContext.User.Identity as ClaimsIdentity);

            if (rsp.Success)
                return Ok(rsp);
            else
                return BadRequest(rsp);
        }

        [HttpPost]
        public async Task<IActionResult> SavePurchase([FromBody] SavePurchaseRequest rq)
        {
            var rsp = await _purchaseService.SavePurchase(rq, HttpContext.User.Identity as ClaimsIdentity);

            if (rsp.Success)
                return Ok(rsp);
            else
                return BadRequest(rsp);
        }

        [HttpPost]
        public async Task<IActionResult> CompletePurchase([FromBody] CompletePurchaseRequest rq)
        {
            var rsp = await _purchaseService.CompletePurchase(rq, HttpContext.User.Identity as ClaimsIdentity);

            if (rsp.Success)
                return Ok(rsp);
            else
                return BadRequest(rsp);
        }
        #endregion
    }
}
