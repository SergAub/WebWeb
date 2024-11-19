using System;
using System.Collections.Generic;

namespace WebWeB.Model;

public partial class Good
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool InStock { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
