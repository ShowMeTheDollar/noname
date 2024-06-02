using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.DTO
{
	public class SearchFilterDto
	{
		public int? CurrentMax { get; set; }
		public int? CurrentMin { get; set; }
		public int? RoomCategory { get; set; }
		public int? Bedrooms { get; set; }
		public int? Beds { get; set; }
		public int? Bathrooms { get; set; }
		public int[]? Amenities { get; set; }
		public int[]? Languages { get; set; }
	}
}
