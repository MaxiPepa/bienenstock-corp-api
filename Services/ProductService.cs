﻿using BienenstockCorpAPI.Data;
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
        public async Task<GetProductsStockResponse> GetProductsStock()
        {
            var products = await _context.Stock
                .Include(x => x.Product)
                .Where(x => x.Quantity > 0)
                .ToListAsync();

            return new GetProductsStockResponse
            {
                Products = products.Select(x => new GetProductsStockResponse.Item
                {
                    ProductId = x.ProductId,
                    Name = x.Product.Name,
                    ProductCode = x.Product.ProductCode,
                    Quantity = x.Quantity,
                    ExpirationDate = x.ExpirationDate,
                }).ToList(),
            };
        }

        public async Task<GetProductCodesResponse> GetProductCodes()
        {
            var products = await _context.Product
                .ToListAsync();

            return new GetProductCodesResponse
            {
                ProductCodes = products.Select(x => new GetProductCodesResponse.Item
                {
                    ProductId = x.ProductId,
                    ProductName = x.Name,
                    ProductCode = x.ProductCode,
                }).ToList(),
            };
        }
        #endregion
    }
}
