using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities;

public partial class Room
{
    public int Id { get; set; }

    public string RoomName { get; set; } = null!;

    public string RoomDescription { get; set; } = null!;

    public int GuestCount { get; set; }

    public int BedroomCount { get; set; }

    public int BedCount { get; set; }

    public int BathroomCount { get; set; }

    public int RoomCategoryId { get; set; }

    public int MemberId { get; set; }

    public bool IsDelete { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? EditedAt { get; set; }

    public string? NearyByTrasportation { get; set; }

    public string? LocationDesription { get; set; }

    public DateTime? CheckInStartTime { get; set; }

    public DateTime? CheckInEndTime { get; set; }

    public DateTime? CheckOutTime { get; set; }

    public decimal FixedPrice { get; set; }

    public decimal? WeekendPrice { get; set; }

    public int RoomStatus { get; set; }

    public string CountryName { get; set; } = null!;

    public string CityName { get; set; } = null!;

    public string Street { get; set; } = null!;

    public string DistrictName { get; set; } = null!;

    public string PostalCode { get; set; } = null!;

    public string? Latitude { get; set; }

    public string? Longitude { get; set; }

    /// <summary>
    /// 自定義排序
    /// </summary>
    public int? Sort { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    public string? Note { get; set; }

    public double? Review { get; set; }

    public virtual Member Member { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<RoomAmenity> RoomAmenities { get; set; } = new List<RoomAmenity>();

    public virtual RoomCategory RoomCategory { get; set; } = null!;

    public virtual ICollection<RoomImage> RoomImages { get; set; } = new List<RoomImage>();

    public virtual ICollection<RoomPrice> RoomPrices { get; set; } = new List<RoomPrice>();

    public virtual ICollection<WishlistDetail> WishlistDetails { get; set; } = new List<WishlistDetail>();
}
