using BienenstockCorpAPI.Data;
using BienenstockCorpAPI.Helpers;
using System.Security.Claims;
using BienenstockCorpAPI.Models.ReportModels;
using Microsoft.EntityFrameworkCore;

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

            var totalProducts = products.Sum(p => p.Quantity);

            var totalPendingProducts = 0;
            purchases.Where(x => x.Pending).ToList().ForEach(p =>
            {
                totalPendingProducts += p.ProductPurchases.Sum(y => y.Quantity);
            });

            return new GetCompanyStatsResponse
            {
                TotalProducts = totalProducts,
                TotalUsers = users.Count,
                TotalPurchases = purchases.Where(x => !x.Pending && !x.Cancelled).ToList().Count,
                TotalSales = sales.Where(x => x.Dispatched).ToList().Count,
                TotalPendingProducts = totalPendingProducts,
                TotalReports = 0,
                Success = true,
                Message = "Successfully retrieved stats",
            };
        }
        #endregion
    }
}