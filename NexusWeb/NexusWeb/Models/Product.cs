using System;
using System.Collections.Generic;

namespace NexusWeb.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string Description { get; set; } = null!;

    public string Detail { get; set; } = null!;

    public int DistributorId { get; set; }

    public int CategoryId { get; set; }

    public string AvatarImages { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual Distributor Distributor { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
