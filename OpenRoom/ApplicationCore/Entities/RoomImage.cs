using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities;

public partial class RoomImage
{
    public int Id { get; set; }

    public string ImageUrl { get; set; } = null!;

    public int RoomId { get; set; }

    public int Sort { get; set; }

    public bool IsDelete { get; set; }

    public virtual Room Room { get; set; } = null!;
}
