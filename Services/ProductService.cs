using BienenstockCorpAPI.Data.Entities;
using BienenstockCorpAPI.Data;
using BienenstockCorpAPI.Models.UserModels;
using Microsoft.EntityFrameworkCore;

namespace BienenstockCorpAPI.Services
{
    public class ProductService
    {
        #region Constructor
        private readonly BienenstockCorpContext _context;

        public ProductService(BienenstockCorpContext context)
        {
            _context = context;
        }
        #endregion

        #region Products
        public async Task<GetProductsResponse> GetProducts()
        {
            var products = await _context.Product
                .ToListAsync();

            return new GetProductsResponse
            {
                Products = products.Select(x => new GetProductsResponse.Item
                {
                    ProductId = x.ProductId,
                    Name = x.Name,
                    ProductCode = x.ProductCode,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    ExpirationDate = x.ExpirationDate,
                    EnterDate = x.EnterDate,
                }).ToList(),
            };
        }
        #endregion
    }
}
