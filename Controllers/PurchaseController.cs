using BienenstockCorpAPI.Models.PurchaseModels;
using BienenstockCorpAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BienenstockCorpAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        [HttpGet("GetPendingPurchases")]
        public async Task<IActionResult> GetPendingPurchases()
        {
            return Ok(await _purchaseService.GetPendingPurchases());
        }

        [HttpPost("SavePurchase")]
        public async Task<IActionResult> SavePurchase([FromBody] SavePurchaseRequest rq)
        {
            var rsp = await _purchaseService.SavePurchase(rq, HttpContext.User.Identity as ClaimsIdentity);

            if (rsp.Success)
                return Ok(rsp);
            else
                return BadRequest(rsp);
        }
        #endregion
    }
}
