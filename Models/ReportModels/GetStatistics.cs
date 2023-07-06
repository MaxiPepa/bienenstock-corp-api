namespace BienenstockCorpAPI.Models.ReportModels
{
    public class GetStatisticsResponse
    {
        public List<Item> Sales { get; set; } = null!;
        public List<Item> Purchases { get; set; } = null!;
        public List<ProductItem> Products { get; set; } = null!;
        public int CancelledSales { get; set; }
        public int CancelledPurchases { get; set; }
        public decimal TotalSaleIncome { get; set; }
        public decimal TotalPurchaseIncome { get; set; }
        public string? Message { get; set; }
        public bool Success { get; set; }

        public class Item
        {
            public string? Date { get; set; }
            public int Quantity { get; set; }
        }

        public class ProductItem
        {
            public string? Name { get; set; }
            public int Quantity { get; set; }
        }
    }
}
