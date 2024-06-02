using System.ComponentModel.DataAnnotations;

namespace OpenRoom.Web.Services.AccountService.DTOs
{
    #nullable disable
    public class ProfilesDTO
    {
        public string AvatarUrl { get; set; }
        //[StringLength(900)]
        public string SelfIntroduction { get; set; }
        //[StringLength(80)]
        public string Job { get; set; }
        //[StringLength(30)]
        public string Live { get; set; }
        //[StringLength(40)]
        public string Obsession { get; set; }
        //[StringLength(40)]
        public string Pet { get; set; }
        public List<string> Languages { get; set; }
    }
}
