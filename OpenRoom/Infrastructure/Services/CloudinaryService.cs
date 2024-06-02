using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using CloudinaryDotNet;
using Microsoft.Extensions.Configuration;
using OpenRoom.Web.Interfaces;
namespace OpenRoom.Web.Services
{
    public class CloudinaryService : IImageFileUploadService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration config)//建構函式，接收一個IConfiguration對象作為參數，從中讀取Cloudinary的配置（雲端名稱、API鍵和API密鑰） 
        {
            var account = new Account(
                config["Cloudinary:cloudname"],
                config["Cloudinary:apikey"],
                config["Cloudinary:apisecret"]
            );

            _cloudinary = new Cloudinary(account);//new Account，初始化Cloudinary instance
            _cloudinary.Api.Secure = true;//確保通過安全的HTTPs連接與Cloudinary服務連結
        }

        //上傳圖片
        private async Task<ImageUploadResult> UploadAsync(IFormFile file, string uploadFolder)//IFormFile（代表要上傳的文件）和uploadFolder（代表在Cloudinary上的資料夾）作為參數
        {
            // 確保 uploadFolder 參數是有效的，你可能需要根據你的項目設置調整此處的路徑
            //var pathFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", uploadFolder, file.FileName);

            //注意：這裡的 uploadFolder 應該是 Cloudinary 上的路徑，不是本地端的路徑
            var pathFile = Path.GetTempFileName(); //C# Path.GetTempFileName()方法生成一個臨時檔案的路徑，此臨時檔案將用於暫存要上傳的圖片資料。

            await using (var stream = new FileStream(pathFile, FileMode.Create))//非同步FileStream
            {
                await file.CopyToAsync(stream);//將IFormFile中的內容資訊複製到這個臨時檔案中。
            }

            //獲取上傳檔案的檔案名
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
            // 生成一個新的唯一文件名(基於原始文件名和一個新的GUID，全球唯一識別符號)，避免在Cloudinary上的命名衝突。
            var uniqueFileName = $"{fileNameWithoutExtension}_{Guid.NewGuid()}";

            // 注意：這裡的 uploadFolder 將成為 Cloudinary 上的資料夾路徑
            var uploadParams = new ImageUploadParams()
            {
                //設定上傳參數，（包括臨時資料夾的位置、目標資料夾和PublicId），然後上傳資料到Cloudinary。
                File = new FileDescription(pathFile),
                Folder = uploadFolder, // 設定目標資料夾的名稱
                PublicId = uniqueFileName // PublicId 不包含資料夾路徑

            };

            //return _cloudinary.Upload(uploadParams);

            //上傳後，刪除之前建的臨時檔案，以清理暫存的資料。
            var uploadResult = _cloudinary.Upload(uploadParams);
            File.Delete(pathFile); // 上傳後刪除臨時的資料
            return uploadResult;//返回上傳結果。

        }

        public async Task<string> UploadImageAsync(IFormFile file, string uploadFolder)
        {
            var result = await UploadAsync(file, uploadFolder);
            return result.SecureUri.AbsoluteUri;
        }
    }
}
