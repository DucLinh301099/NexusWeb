using System;
using System.Collections.Generic;

namespace NexusWeb.Models;

public partial class Booking
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? UserId { get; set; }

    public int ConnectionId { get; set; }

    public string Address { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Message { get; set; } = null!;

    public byte Status { get; set; }

    public virtual Connection Connection { get; set; } = null!;

    public virtual User? User { get; set; }
}
