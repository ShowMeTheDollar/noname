using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

#region ForSignIn
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
// sample --> https://gist.github.com/isdaviddong/8c42a0e226a33131d5af6ab4514e960e
#endregion

namespace __NameSpace__.Pages
{
    public class LoginModel : PageModel
    {
        public IConfigurationSection Channel_ID, Channel_Secret, CallbackURL;

        public LoginModel(IConfiguration config)
        {
            Channel_ID = config.GetSection("LINE-Login-Setting:Channel_ID");
            Channel_Secret = config.GetSection("LINE-Login-Setting:Channel_Secret");
            CallbackURL = config.GetSection("LINE-Login-Setting:CallbackURL");
        }

        public IActionResult OnGet()
        {
            //has login? 
            if (User.Identity.IsAuthenticated)
            {
                //logout
                HttpContext.SignOutAsync();
                //redirect to  login page(self)
                return Redirect("/Login");
            }
            return null;
        }
    }
}
