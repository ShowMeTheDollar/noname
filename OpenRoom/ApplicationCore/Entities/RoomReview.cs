using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities;

public partial class RoomReview
{
    public int Id { get; set; }

    public int RatingScore { get; set; }

    public string ReviewContent { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual Order IdNavigation { get; set; } = null!;
}
