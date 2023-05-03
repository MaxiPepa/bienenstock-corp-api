using System;
using System.Collections.Generic;

namespace BienenstockCorpAPI.Data.Entities;

public partial class ProductPurchase
{
    public int ProductId { get; set; }

    public int PurchaseId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Purchase Purchase { get; set; } = null!;
}
