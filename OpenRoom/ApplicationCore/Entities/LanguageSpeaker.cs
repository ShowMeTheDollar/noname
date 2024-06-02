using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities;

public partial class LanguageSpeaker
{
    public int Id { get; set; }

    public int Language { get; set; }

    public int MemberId { get; set; }

    public virtual Member Member { get; set; } = null!;
}
