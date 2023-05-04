using BienenstockCorpAPI.Data;
using BienenstockCorpAPI.Data.Entities;
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
        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(_productService.GetProducts());
        }
        #endregion
    }
}
