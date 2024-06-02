using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.DTO
{
    public class CalendarPriceDto
    {
        public int RoomId { get; set; }
        public decimal FixedPrice { get; set; }
        public decimal? WeekendPrice { get; set; }
        public decimal? SeparatePrice { get; set; }
        public List<RoomPrice>? individualPrices { get; set; }
    }
}
