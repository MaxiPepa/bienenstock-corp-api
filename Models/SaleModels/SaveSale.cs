namespace BienenstockCorpAPI.Models.SaleModels
{
    public class SaveSaleRequest
    {
        public DateTime SaleDate { get; set; }
        public List<Item> Products { get; set;} = null!;
        public BillItem BillingInformation { get; set; } = null!;

        public class Item
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; } = null!;
            public decimal UnitPrice { get; set; }
            public int Quantity { get; set; }
        }

        public class BillItem
        {
            public string BusinessName { get; set; } = null!;
            public string BillType { get; set; } = null!;
            public string PaymentType { get; set; } = null!;
            public string ConsumerAddress { get; set; } = null!;
            public string ConsumerIdentifier { get; set; } = null!;
        }
    }

    public class SaveSaleResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
