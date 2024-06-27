using System;
using System.Collections.Generic;

namespace NexusWeb.Models;

public partial class ConnectionType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Deposit { get; set; }

    public virtual ICollection<Connection> Connections { get; set; } = new List<Connection>();
}
