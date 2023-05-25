namespace BienenstockCorpAPI.Models.UserModels
{
    public class GetProductsStockResponse
    {
        public List<Item> Products { get; set; } = null!;

        public class Item
        {
            public int ProductId { get; set; }
            public string Name { get; set; } = null!;
            public string ProductCode { get; set; } = null!;
            public DateTime? ExpirationDate { get; set; }
            public int Quantity { get; set; }
        }
    }
}
