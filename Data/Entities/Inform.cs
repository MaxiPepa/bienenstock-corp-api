using System;
using System.Collections.Generic;

namespace BienenstockCorpAPI.Data.Entities;

public partial class Inform
{
    public int InformId { get; set; }

    public string Description { get; set; } = null!;

    public DateTime IssueDate { get; set; }

    public string Title { get; set; } = null!;

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
