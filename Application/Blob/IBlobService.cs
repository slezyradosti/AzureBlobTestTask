using Application.Core;
using Application.DTOs;

namespace Application.BlobService
{
    public interface IBlobService
    {
        public Task<Result<string>> UploadBlobAsync(BlobFormDto blobFormDto, string containerName);
    }
}
