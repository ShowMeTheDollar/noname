using MailKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.DTO
{
    #nullable disable
    public class RoomSourceDto
    {
        public int Id {  get; set; }
        public string RoomName { get; set; }
        public int GuestCount { get; set; }
        public int BedroomCount { get; set; }
        public int BedCount {  get; set; }
        public int BathroomCount {  get; set; }
        public int RoomCategoryID {  get; set; }
        public int MemberID {  get; set; }
        public decimal FixedPrice { get; set; }      
        public string CountryName { get; set; }
        public string CityName { get; set; }
        public string Street {  get; set; }
        public string DistrictName { get; set; }

		public string PostalCode { get; set; }

	}
}
