using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BienenstockCorpAPI.Data.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PassHash { get; set; } = null!;

    public string UserType { get; set; } = null!;

    public string? Avatar { get; set; }

    public bool Inactive { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

    [NotMapped]
    public string FullName => Name + " " + LastName;
}
