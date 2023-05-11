using BienenstockCorpAPI.Data;
using BienenstockCorpAPI.Models.PurchaseModels;
using Microsoft.EntityFrameworkCore;

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
                    }).ToList(),
                }).ToList(),
            };
        }
        #endregion
    }
}
