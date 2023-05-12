namespace BienenstockCorpAPI.Models.ProductModels
{
    public class SaveProductsRequest
    {
        public List<ProductItem> Products { get; set; }

        public class ProductItem
        {
            public string Name { get; set; }
            public string ProductCode { get; set; }
        }
    }

    public class SaveProductsResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
