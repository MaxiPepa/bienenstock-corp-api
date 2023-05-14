namespace BienenstockCorpAPI.Models.PurchaseModels
{
    public class GetPendingPurchasesResponse
    {
        public List<PurchaseItem> Purchases { get; set; }

        public class PurchaseItem
        {
            public int PurchaseId { get; set; }
            public DateTime Date { get; set; }
            public decimal TotalPrice { get; set; }
            public string Supplier { get; set; }
            public List<ProductItem> Products{ get; set; }
        }

        public class ProductItem
        {
            public int ProductId { get; set; }
            public string Name { get; set; }
            public string? ProductCode { get; set; }
        }
    }
}
