namespace BienenstockCorpAPI.Models.ProductModels
{
    public class SaveProductsRequest
    {
        public List<ProductItem> Products { get; set; } = null!;

        public class ProductItem
        {
            public string Name { get; set; } = null!;
            public string ProductCode { get; set; } = null!;
        }
    }

    public class SaveProductsResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
