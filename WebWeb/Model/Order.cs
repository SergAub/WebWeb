using System;
using System.Collections.Generic;

namespace WebWeB.Model;

public partial class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int GoodsId { get; set; }

    public int Count { get; set; }

    public virtual Good Goods { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
