using System;
using System.Collections.Generic;

namespace BienenstockCorpAPI.Data.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PassHash { get; set; } = null!;

    public string UserType { get; set; } = null!;

    public virtual ICollection<Inform> Informs { get; set; } = new List<Inform>();

    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();

    public virtual ICollection<ProductBuyer> ProductBuyers { get; set; } = new List<ProductBuyer>();
}
