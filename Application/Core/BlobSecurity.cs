namespace Application.Core
{
    public record BlobSecurity
    {
        public string StorageAccount { get; init; }
        public string StorageKey { get; init; }
        public string AzureBlobConnectionString { get; init; }
        public string ContainerName { get; init; }
    }
}
