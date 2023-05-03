using System;
using System.Collections.Generic;

namespace BienenstockCorpAPI.Data.Entities;

public partial class Purchase
{
    public int PurchaseId { get; set; }

    public DateTime Date { get; set; }

    public decimal TotalPrice { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<ProductPurchase> ProductPurchases { get; set; } = new List<ProductPurchase>();

    public virtual User User { get; set; } = null!;
}
