using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities;

public partial class Order
{
    public int Id { get; set; }

    public int RoomId { get; set; }

    public DateTime CheckIn { get; set; }

    public DateTime CheckOut { get; set; }

    public int CustomerCount { get; set; }

    public int PaymentMethod { get; set; }

    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// 只有變更才會有值
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    public int MemberId { get; set; }

    public string ReceiptNo { get; set; } = null!;

    public string OrderNo { get; set; } = null!;

    public string? Note { get; set; }

    public int OrderStatus { get; set; }

    public decimal TotalPrice { get; set; }

    public virtual ICollection<Ecpay> Ecpays { get; set; } = new List<Ecpay>();

    public virtual Member Member { get; set; } = null!;

    public virtual Room Room { get; set; } = null!;

    public virtual RoomReview? RoomReview { get; set; }
}
