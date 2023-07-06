using BienenstockCorpAPI.Data;
using BienenstockCorpAPI.Helpers;
using System.Security.Claims;
using BienenstockCorpAPI.Models.ReportModels;
using Microsoft.EntityFrameworkCore;
using BienenstockCorpAPI.Helpers.Consts;

namespace BienenstockCorpAPI.Services
{
    public class ReportService
    {
        #region Constructor
        private readonly BienenstockCorpContext _context;

        public ReportService(BienenstockCorpContext context)
        {
            _context = context;
        }
        #endregion

        #region Report
        public async Task<GetCompanyStatsResponse> GetCompanyStats(ClaimsIdentity? identity)
        {
            var token = identity.TokenVerifier();

            if (!token.Success)
            {
                return new GetCompanyStatsResponse
                {
                    Message = token.Message,
                    Success = false,
                };
            }

            var products = await _context.Stock
                .ToListAsync();

            var users = await _context.User
                .ToListAsync();

            var purchases = await _context.Purchase
                .Include(x => x.ProductPurchases)
                .ToListAsync();

            var sales = await _context.Sale
                .ToListAsync();

            var totalProducts = products.Where(p => p.Quantity > 0).ToList().Count;

            var pendingTransactions = purchases.Where(x => x.Pending).ToList().Count + sales.Where(x => !x.Dispatched && !x.Cancelled).ToList().Count;

            return new GetCompanyStatsResponse
            {
                TotalProducts = totalProducts,
                TotalUsers = users.Count,
                CompletedPurchases = purchases.Where(x => !x.Pending && !x.Cancelled).ToList().Count,
                CompletedSales = sales.Where(x => x.Dispatched).ToList().Count,
                PendingTransactions = pendingTransactions,
                TotalReports = 0,
                Success = true,
                Message = "Successfully retrieved stats",
            };
        }

        public async Task<GetStatisticsResponse> GetStatistics(ClaimsIdentity? identity)
        {
            var token = identity.TokenVerifier();

            if (!token.Success || (token.UserType != UserType.ADMIN && token.UserType != UserType.ANALYST))
            {
                return new GetStatisticsResponse
                {
                    Message = "Inssuficient permissions",
                    Success = false,
                };
            }

            var purchases = await _context.Purchase
                .Include(x => x.ProductPurchases)
                .ToListAsync();

            var sales = _context.Sale
                .Include(x => x.ProductSales)
                .AsQueryable();

            var products = _context.ProductSale
                .GroupBy(x => x.ProductId)
                .Select(y => new
                {
                    Product = y.Key,
                    ProductName = y.First().Product.Name,
                    TotalQuantity = y.Sum(p => p.Quantity),
                })
                .OrderByDescending(x => x.TotalQuantity);

            return new GetStatisticsResponse
            {
                Purchases = purchases.Where(x => !x.Pending && !x.Cancelled).Select(x => new GetStatisticsResponse.Item
                {
                    Date = x.Date.Date.ToString("yyyy-MM-dd"),
                    Quantity = x.ProductPurchases.Sum(x => x.Quantity),
                }).ToList(),
                Sales = sales.Where(x => x.Dispatched).Select(x => new GetStatisticsResponse.Item
                {
                    Date = x.Date.Date.ToString("yyyy-MM-dd"),
                    Quantity = x.ProductSales.Sum(x => x.Quantity),
                }).ToList(),
                Products = products.Select(x => new GetStatisticsResponse.ProductItem
                {
                    Name = x.ProductName,
                    Quantity = x.TotalQuantity,
                }).ToList(),
                CancelledPurchases = purchases.Where(x => x.Cancelled).ToList().Count,
                CancelledSales = purchases.Where(x => x.Cancelled).ToList().Count,
                TotalPurchaseIncome = purchases.Sum(x => x.TotalPrice),
                TotalSaleIncome = sales.Sum(x => x.TotalPrice),
                Success = true,
                Message = "Successfully retrieved statistics",
            };
        }
        #endregion
    }
}