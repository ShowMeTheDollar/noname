namespace ApplicationCore.Enums
{
    /// <summary>
    /// Order訂單的狀態
    /// </summary>
    public enum OrderStatus
    {
        Peding = 0, //已下單,等待付款中
        Active = 1, //已下單,已完成付款
        Expired = 2, //已下單,但超過付款期限
        Canceled = 3, //訂單已取消
        Ongoing = 4, //旅客入住中
        Closed = 5 //旅客已退房,訂單結束
    }
    
}
