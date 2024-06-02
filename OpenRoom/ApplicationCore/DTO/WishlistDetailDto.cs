using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.DTO
{
    public class WishlistDetailDto
    {
        public int WishDetailID {  get; set; }
        public int WishlistID { get; set; }
        public int RoomID { get; set; }
        public string RoomName { get; set; }
        public string RoomCategory { get; set; }
        public int? MemberID { get; set; }
    }
}
