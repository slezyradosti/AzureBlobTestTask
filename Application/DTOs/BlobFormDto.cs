using Microsoft.AspNetCore.Http;

namespace Application.DTOs
{
    public class BlobFormDto
    {
        public string Email { get; set; }
        public IFormFile File { get; set; }
    }
}
