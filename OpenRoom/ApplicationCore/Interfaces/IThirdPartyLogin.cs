using ApplicationCore.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ApplicationCore.Interfaces
{
    public interface IThirdPartyLogin<TUserInfo>
    {
        Task<TUserInfo> GetUserInfoAsync(string accessToken);

        int CheckAndSaveUserInfo(TUserInfo userInfo);
	}

	public interface IGoogleThirdPartyLoginService : IThirdPartyLogin<GoogleUserInfo>
    {

    }

	public interface ILineThirdPartyLoginService : IThirdPartyLogin<LineUserInfo>
	{
        public LineChannelInfo GetLineLoginSettings();

        Task<string> GetAccessTokenAsync(string code);
	}
}
