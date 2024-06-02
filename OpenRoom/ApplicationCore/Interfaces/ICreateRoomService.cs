using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
#nullable disable
    public interface ICreateRoomService
    {
        Task<int> SaveRoomAsync(CreateRoomDto roomCreationDto);
    }
    // Ensure that CreateRoomDto is defined within the same namespace
    public class CreateRoomDto
    {
        //  the properties based on your JSON structure
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "RoomCategory is required.")]
        public int RoomCategory { get; set; }
        [Required]
        public AddressDto Address { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "GuestCount must be at least 1.")]
        public int GuestCount { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "GuestCount must be at least 1.")]
        public int Bedrooms { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "GuestCount must be at least 1.")]
        public int Beds { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "GuestCount must be at least 1.")]
        public int Bathrooms { get; set; }
        [Required]
        public List<int> Amenities { get; set; }
        [Required]
        public List<string> ImageUrls { get; set; }
        [Required]
        public string RoomName { get; set; }
        [Required]
        public string RoomDescription { get; set; }
        [Required]
        public decimal RoomPrice { get; set; }
        [Required]
        public int MemberId { get; set; }
    }

    public class AddressDto
    {
        [Required]
        public string Country { get; set; }
        [Required]
        public string StreetAddress { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string District { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string Latitude { get; set; }
        [Required]
        public string Longitude { get; set; }
    }
}
