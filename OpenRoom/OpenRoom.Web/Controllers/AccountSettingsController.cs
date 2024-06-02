using ApplicationCore.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.IO;
using ApplicationCore.Interfaces;
using OpenRoom.Web.Services.AccountService.DTOs;
using OpenRoom.Web.Services.AccountService;
using Microsoft.IdentityModel.Tokens;
using OpenRoom.Web.Services;
using ApplicationCore.Extensions;
using Microsoft.AspNetCore.Authorization;

#nullable disable
namespace OpenRoom.Web.Controllers
{
    [Authorize]
    public class AccountSettingsController : Controller
    {
        private readonly IAccountViewModelService _accountViewModelService;
        private readonly IMemberRepository _memberRepository;
        private readonly IRepository<Member> _memberRepo;
        private readonly AccountManagerService _accountManagerService;
        private readonly UserService _userService;
        public AccountSettingsController(IAccountViewModelService service, IMemberRepository memberRepository, AccountManagerService accountManagerService, UserService userService, IRepository<Member> memberRepo)
        {
            _accountViewModelService = service;
            _memberRepository = memberRepository;
            _accountManagerService = accountManagerService;
            _userService = userService;
            _memberRepo = memberRepo;
        }

        [Route("Users/Show/{id}")]
        [AllowAnonymous] 
        public async Task<IActionResult> UserProfile(int id, string room)
        {
            if (id <= 0)
            {
                return RedirectToAction("Error", "Index");
            }
            UserProfileViewModel userProfileViewModel = await _accountViewModelService.GetUserProfileViewModel(id);
            ViewData["RoomId"] = room;

            return userProfileViewModel == null ? RedirectToAction("Error", "Index") : View(userProfileViewModel);
        }

        [HttpGet]
        public IActionResult Account()
        {
            AccountDataViewModel accountData = _accountViewModelService.GetAccountCard();
            return View(accountData);
        }

        [HttpGet]
        public async Task<IActionResult> Personal()
        {
            var memberId = _userService.GetMemberId();
            if (memberId <= 0)
            {
                return RedirectToAction("Error", "Index");
            }
            PersonalViewModel personalData = await _accountViewModelService.GetPersonalData(memberId);

            return personalData == null ? RedirectToAction("Error", "Index") : View(personalData);
        }

        [HttpPost]
        public async Task<IActionResult> Personal(Member memberUpdate)
        {
            Member member = await _memberRepository.GetByIdAsync(memberUpdate.Id);

            foreach (var property in typeof(Member).GetProperties()) //PropertyInfo[]
            {
                var updatedValue = property.GetValue(memberUpdate);
                var currentValue = property.GetValue(member);
                if (updatedValue != null && !updatedValue.Equals(currentValue))
                {
                    property.SetValue(member, updatedValue);
                }
            }
            try
            {
                await _memberRepo.UpdateAsync(member);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return RedirectToAction(nameof(Personal));
        }

        [HttpGet]
        public async Task<IActionResult> LoginAndSecurity()
        {
            var memberId = _userService.GetMemberId();
            if (memberId <= 0)
            {
                return RedirectToAction("Error", "Index");
            }
            PersonalViewModel personalData = await _accountViewModelService.GetPersonalData(memberId);

            return personalData == null ? RedirectToAction("Error", "Index") : View(personalData);
        }

        [HttpPost]
        public async Task<IActionResult> LoginAndSecurity(PersonalViewModel memberUpdate)
        {
            Member member = await _memberRepository.GetByIdAsync(memberUpdate.Id);
            if (member.Password != memberUpdate.OldPassword.ToSHA256())
            {
                TempData["passwordChangeResult"] = "oldPasswordDismatch";
                return RedirectToAction(nameof(LoginAndSecurity));
            }

            if(memberUpdate.ConfirmPassword != memberUpdate.Password)
            {
                TempData["passwordChangeResult"] = "confirmPasswordFailure";
                return RedirectToAction(nameof(LoginAndSecurity));
            }

            member.Password = memberUpdate.Password.ToSHA256();
            try
            {
                await _memberRepo.UpdateAsync(member);
                TempData["passwordChangeResult"] = "success";
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return RedirectToAction(nameof(LoginAndSecurity));
        }

        [Route("Users/Show")]
        public async Task<IActionResult> ProfileDetails()
        {
            //int memberId = 3;
            int memberId = _userService.GetMemberId().GetValueOrDefault();

            if (memberId <= 0)
            {
                return RedirectToAction("Error", "Index");
            }
            UserProfileViewModel userProfileViewModel = await _accountViewModelService.GetUserProfileViewModel(memberId);

            return userProfileViewModel == null ? RedirectToAction("Error", "Index") : View(userProfileViewModel);
        }

        [HttpGet]
        [Route("Users/Edit")]
        public async Task<IActionResult> ProfileEdit()
        {
            //int memberId = 3;
            int memberId = _userService.GetMemberId().GetValueOrDefault();
            if (memberId <= 0)
            {
                return RedirectToAction("Error", "Index");
            }
            UserProfileViewModel userProfileViewModel = await _accountViewModelService.GetUserProfileViewModel(memberId);

            return userProfileViewModel == null ? RedirectToAction("Error", "Index") : View(userProfileViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Users/Edit")]
        public async Task<IActionResult> ProfileEdit(ProfilesDTO profiles)
        {
            try
            {
                var editResult = await _accountManagerService.EditProfiles(profiles);
                if (editResult == null)
                {
                    return NotFound();
                }
                return RedirectToAction("ProfileDetails");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            
        }
    }
}