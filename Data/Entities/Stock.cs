using System;
using System.Collections.Generic;

namespace BienenstockCorpAPI.Data.Entities;

public partial class Stock
{
    public int StockId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public virtual Product Product { get; set; } = null!;
}
