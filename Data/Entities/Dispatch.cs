using System;
using System.Collections.Generic;

namespace BienenstockCorpAPI.Data.Entities;

public partial class Dispatch
{
    public int DispatchId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public bool IsDispatched { get; set; }

    public DateTime? DispatchDate { get; set; }

    public virtual Product Product { get; set; } = null!;
}
