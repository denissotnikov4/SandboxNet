using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Sandbox.Controllers.Requests.Attributes;

public class DockerImageNameAttribute : ValidationAttribute
{
    private static readonly Regex DockerImageRegex = new Regex(
        @"^([a-z0-9]+(?:[._-][a-z0-9]+)*(?:/[a-z0-9]+(?:[._-][a-z0-9]+)*)*)$",
        RegexOptions.Compiled);

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is string imageName && DockerImageRegex.IsMatch(imageName))
        {
            return ValidationResult.Success;
        }
        
        return new ValidationResult("Invalid Docker image name. " +
                                    "It must consist of lowercase letters, digits, and may contain '.', '_', or '-'.");
    }
}