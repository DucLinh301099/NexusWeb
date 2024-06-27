using System;
using System.Collections.Generic;

namespace NexusWeb.Models;

public partial class Distributor
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string? Phone { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
