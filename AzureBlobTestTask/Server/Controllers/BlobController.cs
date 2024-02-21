using AzureBlobTestTask.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace AzureBlobTestTask.Server.Controllers
{
    public class BlobController
    {
        [HttpPost]
        public async Task<IActionResult> AddBlob([FromForm] BlobForm blobFormDto)
        {
            
        }
    }
}
