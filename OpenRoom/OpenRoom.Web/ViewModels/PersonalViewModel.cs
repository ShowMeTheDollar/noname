using System.ComponentModel.DataAnnotations;

namespace OpenRoom.Web.ViewModels
{
#nullable disable

	public class PersonalViewModel
	{
		public int Id { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
		public string Email { get; set; }
		public string CountryName { get; set; }
		public string CityName { get; set; }
		public string Street { get; set; }
		public string DistrictName { get; set; }
		public string PostalCode { get; set; }
		public string EmergencyContact { get; set; }
		public string EmergencyNumber { get; set; }
	}

}
