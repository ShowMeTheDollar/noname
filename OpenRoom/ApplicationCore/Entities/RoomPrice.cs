using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities;

public partial class RoomPrice
{
    public int Id { get; set; }

    public int RoomId { get; set; }

    public DateTime Date { get; set; }

    public decimal SeparatePrice { get; set; }

    public virtual Room Room { get; set; } = null!;
}
