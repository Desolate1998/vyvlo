using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;

namespace Common.BloblServiceManager;

public class BlobServiceFileManager(
    ILogger<BlobServiceFileManager> logger,
    BlobServiceManager blobServiceManager) : IBlobServiceFileManager
{
    private BlobContainerClient GetBlobContainerClient(string containerName)
    {
        return blobServiceManager.BlobServiceClient.GetBlobContainerClient(containerName);
    }

    public async Task UploadFileAsync(string containerName, Stream file, string fileName)
    {
        logger.LogInformation($"Uploading file {fileName} to container {containerName}");
        BlobContainerClient containerClient = GetBlobContainerClient(containerName);
        BlobClient blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(file, true);
    }
}
