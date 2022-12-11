using Microsoft.AspNetCore.Http;
using MuscleGain.Core.Contracts;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace MuscleGain.Core.Services.Cloudinary;

public class CloudinaryService : ICloudinaryService
    {
        private readonly CloudinaryDotNet.Cloudinary cloudinary;

        public CloudinaryService(CloudinaryDotNet.Cloudinary cloudinary)
        {
            this.cloudinary = cloudinary;
        }

        public async Task<string> UploudAsync(IFormFile file)
        {
            if (file == null || this.IsFileValid(file) == false)
            {
	            throw new Exception("File cannot be null!");
            }       

            string url = " ";
            byte[] fileBytes;
            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                fileBytes = stream.ToArray();
            }

            using (var uploadStream = new MemoryStream(fileBytes))
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, uploadStream),
                };
                var result = await this.cloudinary.UploadAsync(uploadParams);

                url = result.Uri.AbsoluteUri;
            }

            return url;
        }

        public async Task<string> UploudAsync(byte[] file)
        {
            byte[] imageBytes = file;
            string url = " ";

            using (var uploadStream = new MemoryStream(imageBytes))
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.ToString(), uploadStream),
                };
                var result = await this.cloudinary.UploadAsync(uploadParams);

                url = result.Uri.AbsoluteUri;
            }

            return url;
        }

        public async Task<string> UploadVideoAsync(IFormFile file)
        {
            string url = " ";
            byte[] fileBytes;
            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                fileBytes = stream.ToArray();
            }

            using (var uploadStream = new MemoryStream(fileBytes))
            {
                var uploadParams = new VideoUploadParams()
                {
                    File = new FileDescription(file.ToString(), uploadStream),
                };
                var result = await this.cloudinary.UploadAsync(uploadParams);

                url = result.Uri.AbsoluteUri;
            }

            return url;
        }

        public bool IsFileValid(IFormFile photoFile)
        {
            if (photoFile == null)
            {
                return true;
            }

            string[] validTypes = new string[]
            {
                "image/x-png", "image/gif", "image/jpeg", "image/jpg", "image/png", "image/gif", "image/svg", "video/x-msvideo", "video/mp4",
            };

            if (validTypes.Contains(photoFile.ContentType) == false)
            {
                return false;
            }

            return true;
        }

        public bool IsVideoFileValid(IFormFile photoFile)
        {
            if (photoFile == null)
            {
                return true;
            }

            string[] validTypes = new string[]
            {
                "video/x-msvideo", "video/mp4", "video/x-flv", "video/x-ms-wmv", "video/quicktime",
            };

            if (validTypes.Contains(photoFile.ContentType) == false)
            {
                return false;
            }

            return true;
        }
    }