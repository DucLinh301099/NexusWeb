﻿using System;
using System.Collections.Generic;

namespace NexusWeb.Models;

public partial class ProductImage
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;
}
