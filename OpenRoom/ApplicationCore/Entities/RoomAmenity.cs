using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities;

public partial class RoomAmenity
{
    public int Id { get; set; }

    public int RoomId { get; set; }

    public int AmentityId { get; set; }

    public virtual RoomAmentityCategory Amentity { get; set; } = null!;

    public virtual Room Room { get; set; } = null!;
}
