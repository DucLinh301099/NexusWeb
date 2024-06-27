using System;
using System.Collections.Generic;

namespace NexusWeb.Models;

public partial class Order
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public decimal Total { get; set; }

    public byte Status { get; set; }

    public DateOnly CreateAt { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual User? User { get; set; }
}
