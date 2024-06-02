using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities;

public partial class Member
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public string Email { get; set; } = null!;

    public string? Mobile { get; set; }

    public string? EmergencyNumber { get; set; }

    public string? PhoneNumber { get; set; }

    public string? EmergencyContact { get; set; }

    public string? SelfIntroduction { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? EditAt { get; set; }

    public string Password { get; set; } = null!;

    public string? Job { get; set; }

    public string? Live { get; set; }

    public string? Obsession { get; set; }

    public string? Pet { get; set; }

    public string? Avatar { get; set; }

    public int? AccountStatus { get; set; }

    public string? CountryName { get; set; }

    public string? CityName { get; set; }

    public string? DistrictName { get; set; }

    public string? Street { get; set; }

    public string? PostalCode { get; set; }

    public string? Latitude { get; set; }

    public string? Longitude { get; set; }

    public virtual ICollection<LanguageSpeaker> LanguageSpeakers { get; set; } = new List<LanguageSpeaker>();

    public virtual ICollection<MemberThirdPartyLink> MemberThirdPartyLinks { get; set; } = new List<MemberThirdPartyLink>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();

    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}
