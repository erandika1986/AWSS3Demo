using AWSUtility.Models;
using AWSUtility.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UploadAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly IS3StorageService _s3StorageService;
        private readonly IConfiguration _config;

        public FileUploadController(IS3StorageService s3StorageService, IConfiguration config)
        {
            this._s3StorageService = s3StorageService;
            this._config = config;
        }


        [HttpPost(Name = "UploadFile")]

        public async Task<IActionResult> UploadFile(IFormFile file)
        {

            await using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var fileExt = Path.GetExtension(file.FileName);
            var docName = $"{Guid.NewGuid().ToString()}{fileExt}";

            var s3Obj = new FileObject()
            {
                BucketName = "mys3demo-erandika",
                InputStream = memoryStream,
                Name = docName
            };

            var credentials = new Credentials()
            {
                AccessKey = _config["AwsConfiguration:AWSAccessKey"],
                SecretKey = _config["AwsConfiguration:AWSSecretKey"]
            };

            var result = await _s3StorageService.UploadFileAsync(s3Obj, credentials);
           
            return Ok(result);

        }
    }
}
