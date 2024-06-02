using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities;

public partial class Wishlist
{
    public int Id { get; set; }

    public int MemberId { get; set; }

    public string WishlistName { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;

    public virtual ICollection<WishlistDetail> WishlistDetails { get; set; } = new List<WishlistDetail>();
}
