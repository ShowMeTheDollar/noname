using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.DTO
{
	public class LineChannelInfo
	{
		public string Channel_ID { get; set; }
		public string Channel_Secret { get; set; }
		public string CallbackURL { get; set; }
	}
}
