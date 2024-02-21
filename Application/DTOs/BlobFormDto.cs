using Microsoft.AspNetCore.Http;

namespace Application.DTOs
{
    public class BlobFormDto
    {
        public required string Email { get; set; }
        public required IFormFile File { get; set; }
    }
}
