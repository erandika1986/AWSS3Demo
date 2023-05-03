using Amazon.Runtime;
using Amazon.S3.Model;
using AWSUtility.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWSUtility.Services
{
    public interface IS3StorageService
    {
        Task<ResponseDto> UploadFileAsync(FileObject obj, Credentials awsCredentialsValues);
    }
}
