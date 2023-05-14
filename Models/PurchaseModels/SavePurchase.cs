using System.Security.Claims;

namespace BienenstockCorpAPI.Models.PurchaseModels
{
    public class SavePurchaseRequest
    {
        public string Supplier { get; set; }
        public DateTime PurchaseDate { get; set; }
        public List<ProductItem> Products { get; set;}

        public class ProductItem
        {
            public string Name { get; set; }
            public string ProductCode { get; set; }
            public decimal UnitPrice { get; set; }
            public int Quantity { get; set; }
        }
    }

    public class SavePurchaseResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
