using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenRoom.Web.Interfaces;
using OpenRoom.Web.Services;

namespace OpenRoom.Web.WebAPI
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class UploadImageController : ControllerBase
    {
        private readonly IImageFileUploadService _cloudinaryService;

        public UploadImageController(IImageFileUploadService cloudinaryService)
        {            
            _cloudinaryService = cloudinaryService;
        }


        [HttpPost]
        [Route("upload")]
        public async Task<ActionResult> UploadImages([FromForm] List<IFormFile> files)
        {
            List<string> urls = new List<string>();

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var url = await _cloudinaryService.UploadImageAsync(file, "uploadFolder");
                    urls.Add(url);
                }
            }

            if (!urls.Any())
            {
                return BadRequest("No valid images were uploaded.");
            }

            // 將圖像的URL存儲到前端的cookie或是以其他方式返回給客戶端
            // 這裡僅示例返回URLs列表
            return Ok(urls);
        }

        //[HttpPost("upload")]
        //public async Task<ActionResult> UploadImages([FromForm] List<IFormFile> files)
        //{
        //    if (files.Count == 0)
        //    {
        //        return BadRequest("No files received.");
        //    }

        //    var urls = new List<string>();

        //    foreach (var file in files)
        //    {
        //        if (file.Length > 0)
        //        {
        //            // Assuming you have a method to validate the file size
        //            if (!IsValidFileSize(file.Length))
        //            {
        //                return BadRequest("File size exceeds the limit.");
        //            }

        //            try
        //            {
        //                var uploadFolder = "uploadFolder"; // Example: "user_uploads"
        //                var url = await _cloudinaryService.UploadImageAsync(file, uploadFolder);
        //                urls.Add(url);
        //            }
        //            catch (Exception ex)
        //            {
        //                // Log the exception details
        //                return StatusCode(500, $"Internal server error: {ex.Message}");
        //            }
        //        }
        //    }

        //    if (!urls.Any())
        //    {
        //        return BadRequest("No valid images were uploaded.");
        //    }

        //    return Ok(urls);
        //}

        //private bool IsValidFileSize(long size)
        //{
        //    const long maxSizeInBytes = 10 * 1024 * 1024; // Example: 10 MB
        //    return size <= maxSizeInBytes;
        //}


    }
}
//是formdata from form 是attritube