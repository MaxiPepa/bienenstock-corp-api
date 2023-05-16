using BienenstockCorpAPI.Data;
using BienenstockCorpAPI.Data.Entities;
using BienenstockCorpAPI.Helpers;
using BienenstockCorpAPI.Helpers.Consts;
using BienenstockCorpAPI.Models.ProductModels;
using BienenstockCorpAPI.Models.PurchaseModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BienenstockCorpAPI.Services
{
    public class PurchaseService
    {
        #region Constructor
        private readonly BienenstockCorpContext _context;

        public PurchaseService(BienenstockCorpContext context)
        {
            _context = context;
        }
        #endregion

        #region Purchase
        public async Task<GetPendingPurchasesResponse> GetPendingPurchases()
        {
            var purchases = await _context.Purchase
                .Include(x => x.ProductPurchases)
                .ThenInclude(x => x.Product)
                .Where(x => x.Pending == true)
                .ToListAsync();

            return new GetPendingPurchasesResponse
            {
                Purchases = purchases.Select(x => new GetPendingPurchasesResponse.PurchaseItem
                {
                    PurchaseId = x.PurchaseId,
                    Date = x.Date,
                    TotalPrice = x.TotalPrice,
                    Supplier = x.Supplier,
                    Products = x.ProductPurchases.Select(p => new GetPendingPurchasesResponse.ProductItem
                    {
                        ProductId = p.Product.ProductId,
                        ProductCode = p.Product.ProductCode,
                        Name = p.Product.Name,
                        Quantity = p.Quantity,
                        UnitPrice = p.UnitPrice,
                    }).ToList(),
                }).ToList(),
            };
        }

        public async Task<SavePurchaseResponse> SavePurchase(SavePurchaseRequest rq, ClaimsIdentity? identity)
        {
            // Validations
            var token = identity.TokenVerifier();

            var validation = ValidateSavePurchase(rq, token);

            if (validation != String.Empty)
            {
                return new SavePurchaseResponse
                {
                    Success = false,
                    Message = validation,
                };
            }

            var products = await _context.Product
                .ToListAsync();

            var unregisteredProducts = rq.Products.Where(x => !products.Any(p => p.ProductCode == x.ProductCode)).ToList();

            if (unregisteredProducts.Any())
            {
                await SaveProducts(new SaveProductsRequest
                {
                    Products = unregisteredProducts.Select(x => new SaveProductsRequest.ProductItem
                    {
                        ProductCode = x.ProductCode,
                        Name = x.Name,
                    }).ToList(),
                });
            }

            var newProducts = await _context.Product
                .ToListAsync();

            var purchaseProducts = newProducts.Where(x => rq.Products.Any(p => p.ProductCode == x.ProductCode)).ToList();
            
            decimal purchasePrice = 0;

            rq.Products.ForEach(x => purchasePrice += x.UnitPrice);

            var purchase = new Purchase
            {
                Supplier = rq.Supplier,
                Date = rq.PurchaseDate,
                TotalPrice = purchasePrice,
                Pending = true,
                UserId = token.UserId,
            };

            purchase.ProductPurchases.AddRange(rq.Products.Select(x => new ProductPurchase
            {
                ProductId = purchaseProducts.First(p => p.ProductCode == x.ProductCode).ProductId,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
            }).ToList());

            try
            {
                _context.Purchase.Add(purchase);
                await _context.SaveChangesAsync();

                return new SavePurchaseResponse
                {
                    Success = true,
                    Message = "Purchase successfully added",
                };
            }
            catch (Exception ex)
            {
                return new SavePurchaseResponse
                {
                    Message = ex.Message,
                    Success = false,
                };
            }
        }
        #endregion

        #region Helpers
        public async Task SaveProducts(SaveProductsRequest rq)
        {
            List<Product> newProducts = rq.Products.Select(x => new Product 
            { 
                ProductCode = x.ProductCode, 
                Name= x.Name,
            }).ToList();

            _context.Product.AddRange(newProducts);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Validations
        public static string ValidateSavePurchase(SavePurchaseRequest rq, TokenVerifyResponse token)
        {
            var error = String.Empty;

            // Token
            if (!token.Success) 
            {
                error = token.Message;
            }
            else if (token.UserType != UserType.BUYER)
            {
                error = "You don't have enough permissions";
            }

            // Request
            if (rq == null)
            {
                error = "Invalid Request";
            }
            else if (string.IsNullOrEmpty(rq.Supplier))
            {
                error = "Supplier is a required field";
            }
            else if (rq.Products.Count == 0)
            {
                error = "The purchase must have at least one product";
            }
            else if (rq.Products.Any(x => x.UnitPrice == 0 || x.Quantity == 0))
            {
                error = "Some of the products have invalid price or quantity";
            }

            return error;
        }
        #endregion

    }
}
