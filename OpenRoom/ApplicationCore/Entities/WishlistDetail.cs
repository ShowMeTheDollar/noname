using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities;

public partial class WishlistDetail
{
    public int Id { get; set; }

    public int WishlistId { get; set; }

    public int RoomId { get; set; }

    public virtual Room Room { get; set; } = null!;

    public virtual Wishlist Wishlist { get; set; } = null!;
}
