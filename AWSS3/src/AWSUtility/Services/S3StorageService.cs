using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using AWSUtility.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWSUtility.Services
{
    public class S3StorageService : IS3StorageService
    {
        public S3StorageService()
        {
            
        }

        public async Task<ResponseDto> UploadFileAsync(FileObject obj, Credentials credentials)
        {
            var awsCredential = new BasicAWSCredentials(credentials.AccessKey, credentials.SecretKey);

            var config = new AmazonS3Config()
            {
                RegionEndpoint = Amazon.RegionEndpoint.EUWest2
            };

            var response = new ResponseDto();

            try
            {
                var uploadRequest = new TransferUtilityUploadRequest()
                {
                    InputStream = obj.InputStream,
                    Key = obj.Name,
                    BucketName = obj.BucketName,
                    CannedACL = S3CannedACL.NoACL
                };


                using var client = new AmazonS3Client(awsCredential, config);

                var transferUtility = new TransferUtility(client);

                await transferUtility.UploadAsync(uploadRequest);

                response.StatusCode = 201;
                response.Message = $"{obj.Name} has been uploaded sucessfully";
            }
            catch (AmazonS3Exception s3Ex)
            {
                response.StatusCode = (int)s3Ex.StatusCode;
                response.Message = s3Ex.Message;
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
