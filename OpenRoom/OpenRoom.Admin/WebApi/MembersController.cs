using ApplicationCore.DTO;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace OpenRoom.Admin.WebApi
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly OpenRoomContext _context;
       
        public MembersController(OpenRoomContext context)
        {
            _context = context;
        }
        // GET: api/Members
        [HttpGet]
        public async Task<IActionResult> GetMembers()
        {
            // Fetch members from the database
            var members = await _context.Members.ToListAsync();

            // Transform the data to the DTO
            var memberDto = members.Select(m => new MemberDto
            {
                Id = m.Id,
                FirstName = m.FirstName,
                LastName = m.LastName,
                Email = m.Email,
                Mobile = m.Mobile,
                EmergencyNumber = m.EmergencyNumber,
                PhoneNumber = m.PhoneNumber,
                EmergencyContact = m.EmergencyContact

            }).ToList();

            // Return the DTOs
            return Ok(memberDto);
        }
    }
}
