using Azure.Storage.Blobs;

namespace Common.BloblServiceManager;

public class BlobServiceManager(string ConnectionString)
{
    public virtual BlobServiceClient BlobServiceClient { get { return new BlobServiceClient(ConnectionString); } }
}
