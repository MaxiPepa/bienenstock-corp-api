namespace BienenstockCorpAPI.Models.UserModels
{
    public class GetProductsResponse
    {
        public List<Item> Products { get; set; }

        public class Item
        {
            public int ProductId { get; set; }
            public string Name { get; set; }
            public string ProductCode { get; set; }
            public decimal Price { get; set; }
            public DateTime? ExpirationDate { get; set; }
            public DateTime EnterDate { get; set; }
            public int Quantity { get; set; }
        }
    }
}
