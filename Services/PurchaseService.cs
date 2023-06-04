using BienenstockCorpAPI.Data;
using BienenstockCorpAPI.Data.Entities;
using BienenstockCorpAPI.Helpers;
using BienenstockCorpAPI.Helpers.Consts;
using BienenstockCorpAPI.Models.LogModels;
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
        private readonly LogService _logService;

        public PurchaseService(BienenstockCorpContext context, LogService logService)
        {
            _context = context;
            _logService = logService;
        }
        #endregion

        #region Purchase
        public async Task<GetPurchasesResponse> GetPurchases(GetPurchasesRequest rq, ClaimsIdentity? identity)
        {
            var token = identity.TokenVerifier();

            if (!token.Success ||
                (token.UserType != UserType.BUYER &&
                token.UserType != UserType.DEPOSITOR &&
                token.UserType != UserType.ADMIN))
            {
                return new GetPurchasesResponse
                {
                    Message = "Insufficient permissions",
                    Success = false,
                };
            }

            var query = _context.Purchase
                .Include(x => x.ProductPurchases)
                .ThenInclude(x => x.Product)
                .Include(x => x.User)
                .AsQueryable();

            // Filters
            if (rq.Pending == true)
                query = query.Where(x => x.Pending);

            if (rq.Cancelled == true)
                query = query.Where(x => x.Cancelled);

            if (rq.Completed == true)
                query = query.Where(x => !x.Pending && !x.Cancelled);

            var purchases = await query.ToListAsync();

            return new GetPurchasesResponse
            {
                Purchases = query.Select(x => new GetPurchasesResponse.PurchaseItem
                {
                    PurchaseId = x.PurchaseId,
                    Date = x.Date,
                    TotalPrice = x.TotalPrice,
                    Supplier = x.Supplier,
                    UserFullName = x.User.FullName,
                    Pending = x.Pending,
                    Cancelled = x.Cancelled,
                    EnterDate = x.EnterDate,
                    Products = x.ProductPurchases.Select(p => new GetPurchasesResponse.ProductItem
                    {
                        ProductId = p.Product.ProductId,
                        ProductCode = p.Product.ProductCode,
                        Name = p.Product.Name,
                        Quantity = p.Quantity,
                        UnitPrice = p.UnitPrice,
                    }).ToList(),
                }).OrderByDescending(x => x.Date).ToList(),
                Message = "Purchases retrieved",
                Success = true,
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

            rq.Products.ForEach(x => purchasePrice += x.UnitPrice * x.Quantity);

            var purchase = new Purchase
            {
                Supplier = rq.Supplier,
                Date = rq.PurchaseDate,
                TotalPrice = purchasePrice,
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
                await _logService.CreateLog(new CreateLogRequest
                {
                    UserId = token.UserId,
                    Description = "Has made a new purchase",
                });

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

        public async Task<CompletePurchaseResponse> CompletePurchase(CompletePurchaseRequest rq, ClaimsIdentity? identity)
        {
            var token = identity.TokenVerifier();

            var validation = ValidateAddStock(rq, token);

            if (!string.IsNullOrEmpty(validation))
            {
                return new CompletePurchaseResponse
                {
                    Message = validation,
                    Success = false,
                };
            }

            var purchase = await _context.Purchase
                .Include(x => x.ProductPurchases)
                .FirstOrDefaultAsync(x => x.PurchaseId == rq.PurchaseId);

            if (purchase == null || !purchase.Pending) 
            {
                return new CompletePurchaseResponse
                {
                    Message = "Purchase was not found or it is already completed",
                    Success = false,
                };
            }
            else if (purchase.Cancelled)
            {
                return new CompletePurchaseResponse
                {
                    Message = "The requested purchase is cancelled",
                    Success = false,
                };
            }

            var productPurchasesIds = purchase.ProductPurchases.Select(x => x.ProductId).ToList();

            var stock = await _context.Stock
                .Where(x => productPurchasesIds.Contains(x.ProductId))
                .ToListAsync();

            foreach (var s in stock)
            {
                s.Quantity += purchase.ProductPurchases.First(x => x.ProductId == s.ProductId).Quantity;
            }

            var newStock = purchase.ProductPurchases.Where(x => !stock.Any(s => s.ProductId == x.ProductId)).ToList();

            if (newStock.Any())
            {
                _context.Stock.AddRange(newStock.Select(x => new Stock
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    ExpirationDate = rq.ExpirationDates.FirstOrDefault(ep => ep.ProductId == x.ProductId)?.ExpirationDate,
                }).ToList());
            }

            purchase.Pending = false;
            purchase.EnterDate = rq.EnterDate;

            try
            {
                await _context.SaveChangesAsync();
                await _logService.CreateLog(new CreateLogRequest
                {
                    UserId = token.UserId,
                    Description = "Completed a purchase",
                });

                return new CompletePurchaseResponse
                {
                    Success = true,
                    Message = "Purchase completed",
                };
            }
            catch (Exception ex)
            {
                return new CompletePurchaseResponse
                {
                    Message = ex.Message,
                    Success = false,
                };
            }
        }

        public async Task<CancelPurchaseResponse> CancelPurchase(CancelPurchaseRequest rq, ClaimsIdentity? identity)
        {
            var token = identity.TokenVerifier();

            if (!token.Success)
            {
                return new CancelPurchaseResponse
                {
                    Success = false,
                    Message = token.Message,
                };
            }
            else if (token.UserType != UserType.BUYER)
            {
                return new CancelPurchaseResponse
                {
                    Success = false,
                    Message = "Insufficient permissions",
                };
            }

            var purchase = await _context.Purchase
                .FirstOrDefaultAsync(x => x.PurchaseId == rq.PurchaseId);

            if (purchase == null || !purchase.Pending)
            {
                return new CancelPurchaseResponse
                {
                    Message = "Purchase was not found or it has already arrived",
                    Success = false,
                };
            }
            else if (purchase.Cancelled)
            {
                return new CancelPurchaseResponse
                {
                    Message = "The requested purchase is already cancelled",
                    Success = false,
                };
            }

            purchase.Cancelled = true;
            purchase.Pending = false;

            try
            {
                await _context.SaveChangesAsync();
                await _logService.CreateLog(new CreateLogRequest
                {
                    UserId = token.UserId,
                    Description = "Cancelled a purchase",
                });

                return new CancelPurchaseResponse
                {
                    Success = true,
                    Message = "Purchase cancelled",
                };
            }
            catch (Exception ex)
            {
                return new CancelPurchaseResponse
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

        public static string ValidateAddStock(CompletePurchaseRequest rq, TokenVerifyResponse token)
        {
            var error = String.Empty;

            if (rq == null)
            {
                error = "Invalid request";
            }
            else if (!token.Success || token.UserType != UserType.DEPOSITOR)
            {
                error = "Insufficient permissions";
            }

            return error;
        }
        #endregion

    }
}
