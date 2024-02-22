using Application.BlobService;
using Application.Core;
using AzureBlobTestTask.Server.Controllers;
using AzureBlobTestTask.Server.Models;
using AzureBlobTestTask.Server.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace TestProject
{
    public class BlobControllerTest
    {
        private readonly BlobController _blobController;

        public BlobControllerTest()
        {
            var configuration = new ConfigurationBuilder()
            .AddUserSecrets<BlobControllerTest>()
            .Build();

            BlobSecurity blobSecurity = new BlobSecurity();
            configuration.GetSection("AzureBlob").Bind(blobSecurity);

            var blobService = new BlobService(Options.Create(blobSecurity));

            _blobController = new BlobController(blobService);
        }

        [Fact]
        public async Task PostRequestFileExtentionFail()
        {
            var workingDir = Directory.GetCurrentDirectory();
            string projectDir = Directory.GetParent(workingDir).Parent.Parent.FullName;
            string filePath = $@"{projectDir}\BlobFiles\TestImage.png";
            // Read the file into a byte array
            byte[] fileBytes = File.ReadAllBytes(filePath);

            // Create a MemoryStream from the byte array
            IFormFile trueFormFile;
            IActionResult result = null;
            using (var ms = new MemoryStream(fileBytes))
            {
                // Create an IFormFile instance using FormFile
                trueFormFile = new FormFile(ms, 0, ms.Length, null, Path.GetFileName(filePath));

                var blobFormDto = new BlobForm()
                {
                    Email = "test@mail.com",
                    File = trueFormFile,
                };

                // manual data validation
                _blobController.ValidateModel(blobFormDto);

                result = await _blobController.AddBlob(blobFormDto);
            }

            Assert.False(_blobController.ModelState.IsValid);
        }

        [Fact]
        public async Task PostRequestEmailFail()
        {
            var workingDir = Directory.GetCurrentDirectory();
            string projectDir = Directory.GetParent(workingDir).Parent.Parent.FullName;
            string filePath = $@"{projectDir}\BlobFiles\TestDoc.docx";
            // Read the file into a byte array
            byte[] fileBytes = File.ReadAllBytes(filePath);

            // Create a MemoryStream from the byte array
            IFormFile trueFormFile;
            IActionResult result = null;
            using (var ms = new MemoryStream(fileBytes))
            {
                // Create an IFormFile instance using FormFile
                trueFormFile = new FormFile(ms, 0, ms.Length, null, Path.GetFileName(filePath));

                var blobFormDto = new BlobForm()
                {
                    Email = "tes",
                    File = trueFormFile,
                };

                // manual data validation
                _blobController.ValidateModel(blobFormDto);

                result = await _blobController.AddBlob(blobFormDto);
            }

            Assert.False(_blobController.ModelState.IsValid);
        }

        [Fact]
        public async Task PostRequestSuccess()
        {
            var workingDir = Directory.GetCurrentDirectory();
            string projectDir = Directory.GetParent(workingDir).Parent.Parent.FullName;
            string filePath = $@"{projectDir}\BlobFiles\TestDoc.docx";
            // Read the file into a byte array
            byte[] fileBytes = File.ReadAllBytes(filePath);

            // Create a MemoryStream from the byte array
            IFormFile trueFormFile;
            IActionResult result = null;
            using (var ms = new MemoryStream(fileBytes))
            {
                // Create an IFormFile instance using FormFile
                trueFormFile = new FormFile(ms, 0, ms.Length, null, Path.GetFileName(filePath));

                var blobFormDto = new BlobForm()
                {
                    Email = "test@mail.com",
                    File = trueFormFile,
                };

                result = await _blobController.AddBlob(blobFormDto);
            }
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
