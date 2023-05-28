namespace BienenstockCorpAPI.Models.SaleModels
{
    public class SaveSaleRequest
    {
        public DateTime SaleDate { get; set; }
        public List<Item> Products { get; set;} = null!;

        public class Item
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; } = null!;
            public decimal UnitPrice { get; set; }
            public int Quantity { get; set; }
        }
    }

    public class SaveSaleResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
