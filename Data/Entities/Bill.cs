using System;
using System.Collections.Generic;

namespace BienenstockCorpAPI.Data.Entities;

public partial class Bill
{
    public int BillId { get; set; }

    public string BusinessName { get; set; } = null!;

    public string BillType { get; set; } = null!;

    public string ConsumerAddress { get; set; } = null!;

    public string CompanyAddress { get; set; } = null!;

    public string ConsumerIdentifier { get; set; } = null!;

    public string CompanyIdentifier { get; set; } = null!;

    public DateTime CompanyStart { get; set; }

    public string PaymentType { get; set; } = null!;

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
