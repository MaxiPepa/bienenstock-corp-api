using BienenstockCorpAPI.Data.Entities;
using BienenstockCorpAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BienenstockCorpAPI.Helpers;
using BienenstockCorpAPI.Helpers.Consts;
using BienenstockCorpAPI.Models.SaleModels;
using BienenstockCorpAPI.Models.LogModels;
using BienenstockCorpAPI.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace BienenstockCorpAPI.Services
{
    public class SaleService
    {
        #region Constructor
        private readonly BienenstockCorpContext _context;
        private readonly LogService _logService;
        private readonly IHubContext<PageHub> _pageHub;

        public SaleService(BienenstockCorpContext context, LogService logService, IHubContext<PageHub> pageHub)
        {
            _context = context;
            _logService = logService;
            _pageHub = pageHub;
        }
        #endregion

        #region Sale
        public async Task<GetSalesResponse> GetSales(GetSalesRequest rq, ClaimsIdentity? identity)
        {
            var token = identity.TokenVerifier();

            if (!token.Success ||
                (token.UserType != UserType.SELLER &&
                token.UserType != UserType.DEPOSITOR &&
                token.UserType != UserType.ADMIN &&
                token.UserType != UserType.ANALYST))
            {
                return new GetSalesResponse
                {
                    Message = "Insufficient permissions",
                    Success = false,
                };
            }

            var query = _context.Sale
                .Include(x => x.ProductSales)
                .ThenInclude(x => x.Product)
                .Include(x => x.User)
                .Include(x => x.Bill)
                .AsQueryable();

            // Filters
            if (rq.PendingDispatch == true)
                query = query.Where(x => !x.Dispatched && !x.Cancelled);

            if (rq.Cancelled == true)
                query = query.Where(x => x.Cancelled);

            if (rq.Dispatched == true)
                query = query.Where(x => x.Dispatched);

            var sales = await query.ToListAsync();

            return new GetSalesResponse
            {
                Sales = sales.Select(x => new GetSalesResponse.SaleItem
                {
                    SaleId = x.SaleId,
                    Date = x.Date,
                    TotalPrice = x.TotalPrice,
                    UserFullName = x.User.FullName,
                    Dispatched = x.Dispatched,
                    DispatchDate = x.DispatchDate,
                    Cancelled = x.Cancelled,
                    Products = x.ProductSales.Select(p => new GetSalesResponse.ProductItem
                    {
                        ProductId = p.Product.ProductId,
                        ProductCode = p.Product.ProductCode,
                        Name = p.Product.Name,
                        Quantity = p.Quantity,
                        UnitPrice = p.UnitPrice,
                    }).ToList(),
                    Bill = new GetSalesResponse.BillItem
                    {
                        BillId = x.BillId,
                        BusinessName = x.Bill.BusinessName,
                        BillType = x.Bill.BillType,
                        PaymentType = x.Bill.PaymentType,
                        ConsumerAddress = x.Bill.ConsumerAddress,
                        CompanyAddress = x.Bill.CompanyAddress,
                        ConsumerIdentifier = x.Bill.ConsumerIdentifier,
                        CompanyIdentifier = x.Bill.CompanyIdentifier,
                        CompanyStart = x.Bill.CompanyStart,
                    }
                }).OrderByDescending(x => x.Date).ToList(),
                Message = "Sales retrieved",
                Success = true,
            };
        }

        public async Task<SaveSaleResponse> SaveSale(SaveSaleRequest rq, ClaimsIdentity? identity)
        {
            // Validations
            var token = identity.TokenVerifier();

            var validation = ValidateSaveSale(rq, token);

            if (validation != String.Empty)
            {
                return new SaveSaleResponse
                {
                    Success = false,
                    Message = validation,
                };
            }

            var productsIds = rq.Products.Select(x => x.ProductId).ToList();

            var stock = await _context.Stock
                .Where(x => productsIds.Contains(x.ProductId))
                .ToListAsync();

            foreach (var product in rq.Products)
            {
                var productStock = stock.First(x => x.ProductId == product.ProductId);

                if (productStock.Quantity < product.Quantity)
                {
                    return new SaveSaleResponse
                    {
                        Success = false,
                        Message = "Insufficient stock for product " + product.ProductName,
                    };
                }
                else
                {
                    productStock.Quantity -= product.Quantity;
                }
            }

            decimal salePrice = 0;

            rq.Products.ForEach(x => salePrice += x.UnitPrice * x.Quantity);

            var sale = new Sale
            {
                Date = rq.SaleDate,
                TotalPrice = salePrice,
                UserId = token.UserId,
                Bill = CreateBill(rq.BillingInformation)
            };

            sale.ProductSales.AddRange(rq.Products.Select(x => new ProductSale
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
            }).ToList());

            try
            {
                _context.Sale.Add(sale);
                await _context.SaveChangesAsync();
                await _logService.CreateLog(new CreateLogRequest
                {
                    UserId = token.UserId,
                    Description = "Has made a new sale",
                });

                SaleUpdate(HubCode.SALE);
                ProductUpdate(HubCode.PRODUCT);
                DepositUpdate(HubCode.DESPOSIT);

                return new SaveSaleResponse
                {
                    Success = true,
                    Message = "Sale successfully added",
                };
            }
            catch (Exception ex)
            {
                return new SaveSaleResponse
                {
                    Message = ex.Message,
                    Success = false,
                };
            }
        }

        public async Task<DispatchSaleResponse> DispatchSale(DispatchSaleRequest rq, ClaimsIdentity? identity)
        {
            var token = identity.TokenVerifier();

            var validation = ValidateDispatchSale(rq, token);

            if (!string.IsNullOrEmpty(validation))
            {
                return new DispatchSaleResponse
                {
                    Message = validation,
                    Success = false,
                };
            }

            var sale = await _context.Sale
                .FirstOrDefaultAsync(x => x.SaleId == rq.SaleId);

            if (sale == null || sale.Dispatched)
            {
                return new DispatchSaleResponse
                {
                    Message = "Sale was not found or it is already dispatched",
                    Success = false,
                };
            }
            else if (sale.Cancelled)
            {
                return new DispatchSaleResponse
                {
                    Message = "The requested sale is cancelled",
                    Success = false,
                };
            }
            else if (sale.Date > rq.DispatchDate)
            {
                return new DispatchSaleResponse
                {
                    Message = "The dispatch date can't be before the sale date",
                    Success = false,
                };
            }

            sale.Dispatched = true;
            sale.DispatchDate = rq.DispatchDate;

            try
            {
                await _context.SaveChangesAsync();
                await _logService.CreateLog(new CreateLogRequest
                {
                    UserId = token.UserId,
                    Description = "Dispatched your Sale",
                });

                SaleUpdate(HubCode.SALE);
                DepositUpdate(HubCode.DESPOSIT);

                return new DispatchSaleResponse
                {
                    Success = true,
                    Message = "Sale dispatched",
                };
            }
            catch (Exception ex)
            {
                return new DispatchSaleResponse
                {
                    Message = ex.Message,
                    Success = false,
                };
            }
        }

        public async Task<CancelSaleResponse> CancelSale(CancelSaleRequest rq, ClaimsIdentity? identity)
        {
            var token = identity.TokenVerifier();

            if (!token.Success)
            {
                return new CancelSaleResponse
                {
                    Success = false,
                    Message = token.Message,
                };
            }
            else if (token.UserType != UserType.SELLER)
            {
                return new CancelSaleResponse
                {
                    Success = false,
                    Message = "Insufficient permissions",
                };
            }

            var sale = await _context.Sale
                .Include(x => x.ProductSales)
                .FirstOrDefaultAsync(x => x.SaleId == rq.SaleId);

            if (sale == null || sale.Dispatched || sale.Cancelled)
            {
                return new CancelSaleResponse
                {
                    Message = "Sale was not found or it is already dispatched/cancelled",
                    Success = false,
                };
            }

            var saleProductsIds = sale.ProductSales.Select(x => x.ProductId).ToList();

            var stock = await _context.Stock
                .Where(x => saleProductsIds.Contains(x.ProductId))
                .ToListAsync();

            foreach (var s in stock)
            {
                s.Quantity += sale.ProductSales.First(x => x.ProductId == s.ProductId).Quantity;
            }

            sale.Cancelled = true;

            try
            {
                await _context.SaveChangesAsync();
                await _logService.CreateLog(new CreateLogRequest
                {
                    UserId = token.UserId,
                    Description = "Cancelled a Sale",
                });

                SaleUpdate(HubCode.SALE);
                DepositUpdate(HubCode.DESPOSIT);
                ProductUpdate(HubCode.PRODUCT);

                return new CancelSaleResponse
                {
                    Success = true,
                    Message = "Sale cancelled",
                };
            }
            catch (Exception ex)
            {
                return new CancelSaleResponse
                {
                    Message = ex.Message,
                    Success = false,
                };
            }
        }
        #endregion

        #region Validations
        public static string ValidateSaveSale(SaveSaleRequest rq, TokenVerifyResponse token)
        {
            var error = String.Empty;

            // Token
            if (!token.Success)
            {
                error = token.Message;
            }
            else if (token.UserType != UserType.SELLER)
            {
                error = "You don't have enough permissions";
            }

            // Request
            if (rq == null)
            {
                error = "Invalid Request";
            }
            else if (rq.Products.Count == 0)
            {
                error = "The sale must have at least one product";
            }
            else if (rq.Products.Any(x => x.UnitPrice == 0 || x.Quantity == 0))
            {
                error = "Some of the products have invalid price or quantity";
            }

            return error;
        }

        public static string ValidateDispatchSale(DispatchSaleRequest rq, TokenVerifyResponse token)
        {
            var error = String.Empty;

            // Token
            if (!token.Success)
            {
                error = token.Message;
            }
            else if (token.UserType != UserType.DEPOSITOR)
            {
                error = "You don't have enough permissions";
            }

            // Request
            if (rq == null)
            {
                error = "Invalid Request";
            }

            return error;
        }
        #endregion

        #region Privates
        private Bill CreateBill(SaveSaleRequest.BillItem billData)
        {
            return new Bill
            {
                BusinessName = billData.BusinessName,
                BillType = billData.BillType,
                PaymentType = billData.PaymentType,
                ConsumerAddress = billData.ConsumerAddress,
                ConsumerIdentifier = billData.ConsumerIdentifier,
                CompanyIdentifier = "30-31415926-9",
                CompanyAddress = "Zeballos 1341, Rosario, Santa Fe, Argentina",
                CompanyStart = new DateTime(2023, 4, 27),
            };
        }

        private void SaleUpdate(string hubCode)
        {
            if (string.IsNullOrEmpty(hubCode))
                return;

            var group = _pageHub.Clients.Group(hubCode);

            // Trigger client side update
            if (group != null)
                group.SendAsync("SaleUpdate");
        }

        private void DepositUpdate(string hubCode)
        {
            if (string.IsNullOrEmpty(hubCode))
                return;

            var group = _pageHub.Clients.Group(hubCode);

            // Trigger client side update
            if (group != null)
                group.SendAsync("DepositUpdate");
        }

        private void ProductUpdate(string hubCode)
        {
            if (string.IsNullOrEmpty(hubCode))
                return;

            var group = _pageHub.Clients.Group(hubCode);

            // Trigger client side update
            if (group != null)
                group.SendAsync("ProductUpdate");
        }
        #endregion
    }
}
