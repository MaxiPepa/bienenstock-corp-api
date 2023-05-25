using System.Security.Claims;

namespace BienenstockCorpAPI.Models.PurchaseModels
{
    public class SavePurchaseRequest
    {
        public string Supplier { get; set; } = null!;
        public DateTime PurchaseDate { get; set; }
        public List<ProductItem> Products { get; set;} = null!;

        public class ProductItem
        {
            public string Name { get; set; } = null!;
            public string ProductCode { get; set; } = null!;
            public decimal UnitPrice { get; set; }
            public int Quantity { get; set; }
        }
    }

    public class SavePurchaseResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
