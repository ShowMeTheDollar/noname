using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.DTO
{
    #nullable disable
    public class MemberDto
    {
        public int Id {  get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string EmergencyNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string EmergencyContact { get; set; }
    }
}
