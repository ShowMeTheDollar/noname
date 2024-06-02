using OpenRoom.Admin.Models;
using OpenRoom.Admin.WebApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OpenRoom.Admin.Filters
{
    public class ApiExceptionFilter : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context) //當exception發生時，我要吐一個固定的值發出去
        {
            context.Result = new OkObjectResult(new BaseApiResponse() { Message = context.Exception.Message });//exception裡面的訊息
            
        }
    }
}
