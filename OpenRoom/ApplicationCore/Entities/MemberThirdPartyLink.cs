using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities;

public partial class MemberThirdPartyLink
{
    public int Id { get; set; }

    public int MemberId { get; set; }

    public int ThirdPartyId { get; set; }

    public virtual Member Member { get; set; } = null!;

    public virtual ThirdPartyLogin ThirdParty { get; set; } = null!;
}
