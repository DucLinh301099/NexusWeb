using System;
using System.Collections.Generic;

namespace NexusWeb.Models;

public partial class Review
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string Comment { get; set; } = null!;

    public int ProductId { get; set; }

    public DateOnly CreateAt { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User? User { get; set; }
}
