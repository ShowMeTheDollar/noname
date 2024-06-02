
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OpenRoom.Web.ViewModels.Hosting
{
#nullable disable
    public class SourceViewModel
    {
        public string SourceCount { get; set; }
        public List<RoomSource> Sources { get; set; }
     

    }

    public class RoomSource
    {
        public int Id { get; set; }
        public string ImgUrl { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string State { get; set; }
        public int CustomerCount { get; set; }
        public int BedRoomCount { get; set; }
        public int BedCount { get; set; }
        public int BathroomCount { get; set; }

        public string District { get; set; }
        public string City { get; set; }
        public List<string> Services { get; set; }
        

    }
}
