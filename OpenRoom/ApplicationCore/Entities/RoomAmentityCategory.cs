using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities;

public partial class RoomAmentityCategory
{
    public int Id { get; set; }

    public string AmentityName { get; set; } = null!;

    public string? Icon { get; set; }

    public int AmentityTypeId { get; set; }

    public virtual AmentityType AmentityType { get; set; } = null!;

    public virtual ICollection<RoomAmenity> RoomAmenities { get; set; } = new List<RoomAmenity>();
}
