﻿namespace BienenstockCorpAPI.Models.PurchaseModels
{
    public class GetPurchasesRequest
    {
        public bool? Pending { get; set; }
        public bool? Cancelled { get; set; }
        public bool? Completed { get; set; }
    }

    public class GetPurchasesResponse
    {
        public List<PurchaseItem> Purchases { get; set; } = null!;
        public string? Message { get; set; }
        public bool Success { get; set; }

        public class PurchaseItem
        {
            public int PurchaseId { get; set; }
            public DateTime Date { get; set; }
            public decimal TotalPrice { get; set; }
            public string Supplier { get; set; } = null!;
            public bool Pending { get; set; }
            public bool Cancelled { get; set; }
            public DateTime? EnterDate { get; set; }
            public string UserFullName { get; set; } = null!;
            public List<ProductItem> Products{ get; set; } = null!;
        }

        public class ProductItem
        {
            public int ProductId { get; set; }
            public string Name { get; set; } = null!;
            public string ProductCode { get; set; } = null!;
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set;}
        }
    }
}
