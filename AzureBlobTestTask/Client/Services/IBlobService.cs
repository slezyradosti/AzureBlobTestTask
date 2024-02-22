using Application.DTOs;

namespace AzureBlobTestTask.Client.Services
{
    public interface IBlobService
    {
        public Task<HttpResponseMessage> UploadBlobAsync(BlobFormDto blobFormDto);
    }
}
