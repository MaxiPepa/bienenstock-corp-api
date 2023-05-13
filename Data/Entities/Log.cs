using System;
using System.Collections.Generic;

namespace BienenstockCorpAPI.Data.Entities;

public partial class Log
{
    public int LogId { get; set; }

    public string Description { get; set; } = null!;

    public int UserId { get; set; }

    public DateTime Date { get; set; }

    public virtual User User { get; set; } = null!;
}
