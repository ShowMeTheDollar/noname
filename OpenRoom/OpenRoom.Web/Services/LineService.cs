using OpenRoom.Web.ViewModels;
using Microsoft.Extensions.Configuration;

namespace OpenRoom.Web.Services
{
    public class LineService
    {
        private readonly IConfiguration _configuration;

        public LineService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public LineChannelViewModel GetLogin()
        {
            return new LineChannelViewModel()
            {
                Channel_ID = _configuration.GetSection("LINE-Login-Setting")["Channel_ID"],
                Channel_Secret = _configuration.GetSection("LINE-Login-Setting")["Channel_Secret"],
                CallbackURL = _configuration.GetSection("LINE-Login-Setting")["CallbackURL"]
            };
        }
    }
}
