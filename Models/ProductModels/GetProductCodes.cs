namespace BienenstockCorpAPI.Models.UserModels
{
    public class GetProductCodesResponse
    {
        public List<Item> ProductCodes { get; set; }

        public class Item
        {
            public int ProductId { get; set; }
            public string? ProductCode { get; set; }
            public string ProductName { get; set; }
        }
    }
}
