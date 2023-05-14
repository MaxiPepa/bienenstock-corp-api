using BienenstockCorpAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BienenstockCorpAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        #region Constructor
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }
        #endregion

        #region Endpoints
        [HttpGet("GetProductsStock")]
        public async Task<IActionResult> GetProductsStock()
        {
            return Ok(await _productService.GetProductsStock());
        }
        #endregion

    }
}
