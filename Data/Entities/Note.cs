using System;
using System.Collections.Generic;

namespace BienenstockCorpAPI.Data.Entities;

public partial class Note
{
    public int NoteId { get; set; }

    public string Description { get; set; } = null!;

    public DateTime Date { get; set; }

    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}
