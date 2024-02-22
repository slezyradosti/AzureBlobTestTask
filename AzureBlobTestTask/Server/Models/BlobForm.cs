using System.ComponentModel.DataAnnotations;
using FileExtensionsAttribute = AzureBlobTestTask.Server.Extensions.FileExtensionsAttribute;

namespace AzureBlobTestTask.Server.Models
{
    public class BlobForm
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Use correct email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "File is required")]
        [FileExtensions(".docx", ErrorMessage = "File extension must be '.docx'")]
        public IFormFile File { get; set; }
    }
}
