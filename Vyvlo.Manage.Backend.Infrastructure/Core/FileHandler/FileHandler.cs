using Common.BloblServiceManager;
using Microsoft.AspNetCore.Http;
using Vyvlo.Manage.Backend.Domain.Interfaces;

namespace Vyvlo.Manage.Backend.Infrastructure.Core.FileHandler;

internal class FileHandler(IBlobServiceFileManager blobService) : IFileHandler
{
    public async Task UploadFileAsync(IFormFile file, string path, string containerName, string? newFileName = null)
    {
        using var fileStream = file.OpenReadStream();
        string filePath = Path.Combine(path, newFileName is null ? file.FileName : newFileName);
        await blobService.UploadFileAsync(containerName, fileStream, filePath);
    }

    public async Task UploadMultpleFileAsync(IEnumerable<IFormFile> files, string path, string containerName)
    {
        foreach (var file in files)
        {
            await UploadFileAsync(file, path, containerName);
        }
    }
}