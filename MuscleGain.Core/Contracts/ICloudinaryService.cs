using Microsoft.AspNetCore.Http;

namespace MuscleGain.Core.Contracts;

public interface ICloudinaryService
{
    bool IsFileValid(IFormFile photoFile);

    bool IsVideoFileValid(IFormFile photoFile);

    Task<string> UploudAsync(IFormFile file);

    Task<string> UploadVideoAsync(IFormFile file);

    Task<string> UploudAsync(byte[] file);
}