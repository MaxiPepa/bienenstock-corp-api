namespace BienenstockCorpAPI.Models.SaleModels
{
    public class GetSalesRequest
    {
        public bool? PendingDispatch { get; set; }
        public bool? Cancelled { get; set; }
        public bool? Dispatched { get; set; }
    }

    public class GetSalesResponse
    {
        public List<SaleItem> Sales { get; set; } = null!;
        public string? Message { get; set; }
        public bool Success { get; set; }

        public class SaleItem
        {
            public int SaleId { get; set; }
            public DateTime Date { get; set; }
            public decimal TotalPrice { get; set; }
            public bool Dispatched { get; set; }
            public DateTime? DispatchDate { get; set; }
            public bool Cancelled { get; set; }
            public string UserFullName { get; set; } = null!;
            public List<ProductItem> Products{ get; set; } = null!;
            public BillItem Bill { get; set; } = null!;
        }

        public class ProductItem
        {
            public int ProductId { get; set; }
            public string Name { get; set; } = null!;
            public string ProductCode { get; set; } = null!;
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set;}
        }

        public class BillItem
        {
            public int BillId { get; set; }
            public string BusinessName { get; set; } = null!;
            public string BillType { get; set; } = null!;
            public string PaymentType { get; set; } = null!;
            public string ConsumerAddress { get; set; } = null!;
            public string CompanyAddress { get; set; } = null!;
            public string ConsumerIdentifier { get; set; } = null!;
            public string CompanyIdentifier { get; set; } = null!;
            public DateTime CompanyStart { get; set; }
        }
    }
}
