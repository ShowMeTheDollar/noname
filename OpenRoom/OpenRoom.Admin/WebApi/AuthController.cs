using System.Runtime.CompilerServices;
using OpenRoom.Admin.Constants;
using OpenRoom.Admin.Filters;
using OpenRoom.Admin.Helpers;
using OpenRoom.Admin.Models;
using OpenRoom.Admin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OpenRoom.Admin.WebApi;

//[ApiController]
//[Route("/api/[controller]/[action]")]
[ApiExceptionFilter]
public class AuthController : BaseApiController //可用繼承的方式去做
{
    private readonly JwtHelper _jwt;
    private readonly UserMangerService _userMangerService;

    public AuthController(JwtHelper jwt, UserMangerService userMangerService)
    {
        _jwt = jwt;
        _userMangerService = userMangerService;
    }

    [HttpPost]
    [AllowAnonymous]
    public IActionResult Login(LoginInDTO request)
    {

        if (!_userMangerService.IsUserValid(request))
        {
            return Ok(new BaseApiResponse());//isSucess 是false，body是null
        }

        return Ok(new BaseApiResponse(_jwt.GenerateToken(request.UserName, 10)));//把產出來的token吐出去，10指10分鐘
        
        //try
        //{
        //    if (!_userMangerService.IsUserValid(request))
        //    {
        //        return Ok(new BaseApiResponse());//isSucess 是false，body是null
        //    }

        //    return Ok(new BaseApiResponse(_jwt.GenerateToken(request.UserName, 10)));//把產出來的token吐出去，10指10分鐘
        //}
        //catch (Exception ex)
        //{
        //    return Ok(new BaseApiResponse()
        //    {

        //        Message = ex.Message
        //    });
        //}
       
    }

    [HttpGet]
    [Authorize(Roles = AuthRole.Admin)]//角色的擴充，const按下編譯，build後自動換值。const值有變，全部要重新建置過跟readonly不同，readonly是在runtime
    public IActionResult GetUserName()
    {
        return Ok(new BaseApiResponse(_userMangerService.GetUserName()));//不管怎麼樣都回傳200
    }

    [HttpGet]
    // [Authorize(Roles = "SuperAdmin")]
    [Authorize(Roles = AuthRole.SuperAdmin)]
    public IActionResult GetClaims()
    {
        return Ok(new BaseApiResponse(User.Claims.Select(x => new { x.Type, x.Value })));//不管怎麼樣都回傳200
    }

    
}

public class LoginInDTO
{
    public string UserName { get; set; }
    public string Password { get; set; }
}