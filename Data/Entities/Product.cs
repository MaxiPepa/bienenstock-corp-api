using System;
using System.Collections.Generic;

namespace BienenstockCorpAPI.Data.Entities;

public partial class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public string? ProductCode { get; set; }

    public decimal Price { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public DateTime EnterDate { get; set; }

    public int Quantity { get; set; }

    public virtual ICollection<Dispatch> Dispatches { get; set; } = new List<Dispatch>();

    public virtual ICollection<ProductPurchase> ProductPurchases { get; set; } = new List<ProductPurchase>();

    public virtual ICollection<ProductSale> ProductSales { get; set; } = new List<ProductSale>();
}
