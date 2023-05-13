using System;
using System.Collections.Generic;

namespace BienenstockCorpAPI.Data.Entities;

public partial class Message
{
    public int MessageId { get; set; }

    public string Desciption { get; set; } = null!;

    public int UserId { get; set; }

    public DateTime Date { get; set; }

    public virtual User User { get; set; } = null!;
}
