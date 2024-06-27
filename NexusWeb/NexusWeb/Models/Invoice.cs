using System;
using System.Collections.Generic;

namespace NexusWeb.Models;

public partial class Invoice
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public decimal TotalInvoice { get; set; }

    public int OrderId { get; set; }

    public DateOnly CreateAt { get; set; }

    public byte Status { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual User? User { get; set; }
}
