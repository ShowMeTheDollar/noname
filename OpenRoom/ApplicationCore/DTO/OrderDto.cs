using MailKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.DTO
{
    #nullable disable
    public class OrderDto
    {
        public int Id { get; set; }
        public int RoomID {  get; set; }
        public int CustomerCount {  get; set; }
        public int MemberID { get; set; }
        public string ReceiptNo {  get; set; }
        public string OrderNo { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
