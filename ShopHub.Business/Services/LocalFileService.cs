using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.X509;
using ShopHub.Business.Interfaces.Services;

namespace ShopHub.Business.Services;

public class LocalFileService(IWebHostEnvironment environment) : IFileService
{
    private readonly IWebHostEnvironment _environment = environment;

    public async Task<string?> UploadAsync(IFormFile file, string folder, CancellationToken cancellationToken = default)
    {
        if (file is null || file.Length == 0)
            return null;

        folder = folder.Replace('\\', '/');

        var uploadFolder = Path.Combine(
            _environment.WebRootPath,
            folder);

        Directory.CreateDirectory(uploadFolder);

        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{extension}";

        var filePath = Path.Combine(uploadFolder, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);

        await file.CopyToAsync(stream, cancellationToken);

        return Path.Combine(folder, fileName).Replace('\\', '/');
    }

    public Task DeleteAsync(string? relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            return Task.CompletedTask;

        var fullPath = Path.Combine(
            _environment.WebRootPath,
            relativePath);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        return Task.CompletedTask;
    }
}
