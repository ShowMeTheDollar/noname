using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities;

public partial class RoomCategory
{
    public int Id { get; set; }

    public string RoomCategory1 { get; set; } = null!;

    public string? Icon { get; set; }

    public int? Sort { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
