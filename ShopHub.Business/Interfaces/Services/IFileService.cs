using Microsoft.AspNetCore.Http;

namespace ShopHub.Business.Interfaces.Services;

public interface IFileService
{
    Task<string?> UploadAsync(IFormFile file, string folder, CancellationToken cancellationToken = default);
    Task DeleteAsync(string? relativePath);
}