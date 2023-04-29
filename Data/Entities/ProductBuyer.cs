using System;
using System.Collections.Generic;

namespace BienenstockCorpAPI.Data.Entities;

public partial class ProductBuyer
{
    public int ProductId { get; set; }

    public int UserId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
