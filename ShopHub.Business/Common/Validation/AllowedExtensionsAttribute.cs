using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

public sealed class AllowedExtensionsAttribute : ValidationAttribute
{
    private readonly string[] _extensions;

    public AllowedExtensionsAttribute(string[] extensions)
    {
        _extensions = extensions;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not IFormFile file)
            return ValidationResult.Success;

        var extension = Path.GetExtension(file.FileName);

        if (!_extensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
        {
            return new ValidationResult(
                ErrorMessage ?? $"Only the following extensions are allowed: {string.Join(", ", _extensions)}");
        }

        return ValidationResult.Success;
    }
}