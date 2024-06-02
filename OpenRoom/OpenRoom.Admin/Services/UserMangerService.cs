using OpenRoom.Admin.WebApi;

namespace OpenRoom.Admin.Services;

public class UserMangerService
{
    private readonly HttpContext _httpContext;
    public UserMangerService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContext = httpContextAccessor?.HttpContext ?? throw new ArgumentNullException(nameof(httpContextAccessor));//確定一定要有值，不然就
    }
    public bool IsUserValid(LoginInDTO request)//下面沒特別處裡，寫在app裡面，後台user可以寫死
    {
        //TODO 轉寫實際的User判斷邏輯，這裡只是簡單的範例，只要有輸入值就讓它過
        //return !string.IsNullOrEmpty(request.UserName) && !string.IsNullOrEmpty(request.Password);
        return request.UserName == "admin" && request.Password == "admin";
    }
    
    public bool IsAuthenticated()
    {
        return _httpContext.User.Identity?.IsAuthenticated ?? false;
    }
    
    public string GetUserName()
    {
        if (!IsAuthenticated()) return string.Empty;
        
        return _httpContext.User.Identity?.Name ?? string.Empty;
    }
}