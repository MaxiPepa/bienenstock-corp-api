using BienenstockCorpAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        #endregion
    }
}
