using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.DTO
{
    public class UpdateWishlistNameApiDto
    {
        public int WishlistId {  get; set; }

        public string WishlistName { get; set; }
    }
}
