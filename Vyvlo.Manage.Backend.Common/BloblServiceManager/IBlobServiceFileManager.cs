namespace Common.BloblServiceManager;

public interface IBlobServiceFileManager
{
    Task UploadFileAsync(string containerName, Stream file, string fileName);
}
