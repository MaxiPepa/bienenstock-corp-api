using System;
using System.Collections.Generic;

namespace BienenstockCorpAPI.Data.Entities;

public partial class ProductSale
{
    public int ProductId { get; set; }

    public int SaleId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Sale Sale { get; set; } = null!;
}
