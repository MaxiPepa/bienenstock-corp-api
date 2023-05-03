﻿using System;
using System.Collections.Generic;

namespace BienenstockCorpAPI.Data.Entities;

public partial class Sale
{
    public int SaleId { get; set; }

    public DateTime Date { get; set; }

    public decimal TotalPrice { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<ProductSale> ProductSales { get; set; } = new List<ProductSale>();

    public virtual User User { get; set; } = null!;
}
