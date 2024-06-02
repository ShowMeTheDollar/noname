using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities;

public partial class AmentityType
{
    public int Id { get; set; }

    public string AmentityName { get; set; } = null!;

    public bool? SpecificInfo { get; set; }

    public int? Sort { get; set; }

    public virtual ICollection<RoomAmentityCategory> RoomAmentityCategories { get; set; } = new List<RoomAmentityCategory>();
}
