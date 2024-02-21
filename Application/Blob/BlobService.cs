using Application.Core;
using Application.DTOs;
using Azure.Storage;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Application.BlobService
{
    public class BlobService : IBlobService
    {
        private readonly BlobSecurity _blobSecurity;
        private readonly CloudStorageAccount _storageAccount;

        public BlobService(IOptions<BlobSecurity> options)
        {
            _blobSecurity = options.Value;
            _storageAccount = CloudStorageAccount.Parse(options.Value.AzureBlobConnectionString);
        }

        public async Task<Result<string>> UploadBlobAsync(BlobFormDto blobFormDto, string containerName)
        {
            var blobName = blobFormDto.File.FileName;
            var file = blobFormDto.File;
            var recipientEmail = blobFormDto.Email;

            if (string.IsNullOrEmpty(blobName))
                return Result<string>.Failure("Blob name email value cannot be null/empty");
            if (string.IsNullOrEmpty(containerName))
                return Result<string>.Failure("Container name value cannot be null/empty");
            if (file == null)
                return Result<string>.Failure("File value cannot be null");


            CloudBlobClient blobClient = _storageAccount.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = blobClient.GetContainerReference(containerName.ToLower());
            CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(blobName);

            try
            {
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    await blockBlob.UploadFromStreamAsync(ms);
                }

                var sasTocken = GetBlobSASTokenByFile(blobName, containerName);
                if (sasTocken == null) Result<string>.Failure("Unable to create SAS token");

                var blobUrl = blockBlob.StorageUri.PrimaryUri + "?" + sasTocken;

                await SetMedataToBlob(blockBlob, recipientEmail, blobUrl);

                return Result<string>.Success("");
            }
            catch (Exception e)
            {
                return Result<string>.Failure(e.Message);
            }
        }

        private async Task SetMedataToBlob(CloudBlockBlob cloudBlockBlob, string email, string blobUrl)
        {
            try
            {
                cloudBlockBlob.Metadata["email"] = email;
                cloudBlockBlob.Metadata["fileLink"] = blobUrl;
                await cloudBlockBlob.SetMetadataAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public string GetBlobSASTokenByFile(string fileName, string containerName)
        {
            try
            {
                var azureStorageAccount = _blobSecurity.StorageAccount;
                var azureStorageAccessKey = _blobSecurity.StorageKey;
                Azure.Storage.Sas.BlobSasBuilder blobSasBuilder = new Azure.Storage.Sas.BlobSasBuilder()
                {
                    BlobContainerName = containerName,
                    BlobName = fileName,
                    ExpiresOn = DateTime.UtcNow.AddHours(1),
                };

                blobSasBuilder.SetPermissions(Azure.Storage.Sas.BlobSasPermissions.Read);
                var sasToken = blobSasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(azureStorageAccount,
                    azureStorageAccessKey)).ToString();
                return sasToken;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
