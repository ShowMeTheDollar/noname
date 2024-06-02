using ApplicationCore.Entities;
using ApplicationCore.Enums;
using ApplicationCore.Interfaces;
using isRock.LineBot;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using OpenRoom.Web.Services.AccountService.DTOs;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace OpenRoom.Web.Services.AccountService
{
    public class AccountManagerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly int _memberId;
        public AccountManagerService(IUnitOfWork unitOfWork, UserService userService)
        {
            _unitOfWork = unitOfWork;
            _memberId = userService.GetMemberId().GetValueOrDefault();
            //_memberId = 3;
        }

        public async Task<object> EditProfiles(ProfilesDTO profiles)
        {
            try
            {
                await _unitOfWork.BeginAsync();

                var member = _unitOfWork.GetRepository<Member>().FirstOrDefault(m => m.Id == _memberId);
                if (member == null)
                {
                    return null;
                }
                
                member.Avatar = profiles.AvatarUrl;
                member.SelfIntroduction = profiles.SelfIntroduction;
                member.Job = profiles.Job;
                member.Live = profiles.Live;
                member.Obsession = profiles.Obsession;
                member.Pet = profiles.Pet;
                _unitOfWork.GetRepository<Member>().Update(member);

                var memberRelationLanguages = _unitOfWork.GetRepository<LanguageSpeaker>().List(l => l.MemberId == _memberId);
                _unitOfWork.GetRepository<LanguageSpeaker>().DeleteRange(memberRelationLanguages);
                if (profiles.Languages != null)
                {
                    foreach (var language in profiles.Languages)
                    {
                        var languageSpeaker = new LanguageSpeaker
                        {
                            Language = (int)Enum.Parse(typeof(Languages), language),
                            MemberId = _memberId
                        };
                        _unitOfWork.GetRepository<LanguageSpeaker>().Add(languageSpeaker);
                    }

                    await _unitOfWork.CommitAsync();
                    return Task.CompletedTask;
                }

                await _unitOfWork.CommitAsync();
                return Task.CompletedTask;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception("系統錯誤，請重新操作");
            }
        }
    }
}
