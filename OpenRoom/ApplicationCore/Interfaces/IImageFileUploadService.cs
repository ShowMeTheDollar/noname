using Microsoft.AspNetCore.Http;

namespace OpenRoom.Web.Interfaces
{
    public interface IImageFileUploadService
    {
        Task<string> UploadImageAsync(IFormFile file, string uploadFolder);
    }
}
