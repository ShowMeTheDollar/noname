using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.DTO
{
    public class UpdateWishlistDto
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string WishlistName { get; set; }
    }
}
