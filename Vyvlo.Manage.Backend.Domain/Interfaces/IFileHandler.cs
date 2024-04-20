using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vyvlo.Manage.Backend.Domain.Interfaces;

public interface IFileHandler
{
    Task UploadFileAsync(IFormFile file, string path, string containerName, string? newFileName = default);
    Task UploadMultpleFileAsync(IEnumerable<IFormFile> files, string path, string containerName);
}
