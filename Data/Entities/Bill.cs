using System;
using System.Collections.Generic;

namespace BienenstockCorpAPI.Data.Entities;

public partial class Bill
{
    public int BillId { get; set; }

    public string Description { get; set; } = null!;

    public DateTime IssueDate { get; set; }

    public string Title { get; set; } = null!;

    public int UserId { get; set; }

    public string BillType { get; set; } = null!;

    public string Reason { get; set; } = null!;

    public string Address { get; set; } = null!;

    public int SaleId { get; set; }

    public virtual Sale Sale { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
