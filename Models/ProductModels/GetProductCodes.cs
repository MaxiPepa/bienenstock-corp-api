namespace BienenstockCorpAPI.Models.UserModels
{
    public class GetProductCodesResponse
    {
        public List<Item> ProductCodes { get; set; } = null!;

        public class Item
        {
            public int ProductId { get; set; }
            public string ProductCode { get; set; } = null!;
            public string ProductName { get; set; } = null!;
        }
    }
}
