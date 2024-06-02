using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities;

public partial class ThirdPartyLogin
{
    public int Id { get; set; }

    public string Provider { get; set; } = null!;

    public string ProviderUserId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<MemberThirdPartyLink> MemberThirdPartyLinks { get; set; } = new List<MemberThirdPartyLink>();
}
