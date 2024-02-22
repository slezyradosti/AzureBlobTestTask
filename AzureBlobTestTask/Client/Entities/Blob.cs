using Application.DTOs;
using AzureBlobTestTask.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using System.Text.Json;

namespace AzureBlobTestTask.Client.Entities
{
    public class Blob
    {
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }
        public BlobFormDto blobFormDto { get; set; }

        [Inject]
        private IBlobService _blobService { get; set; }

        public void HandleFailedRequest()
        {
            ErrorMessage = "Something went wrong, form not submited.";
        }

        public async Task HandleValidRequestAsync()
        {
            if (blobFormDto == null || blobFormDto.File == null || blobFormDto.Email == null)
            {
                ErrorMessage = "All the fields are required!";
                return;
            }

            var response = await _blobService.UploadBlobAsync(blobFormDto);

            if (response == null) ErrorMessage = "Something went wrong, form not submited.";
            else if (response.StatusCode == HttpStatusCode.OK)
            {
                SuccessMessage = "File successfully uploaded! Check your email.";
                return;
            }
            else
            {
                StringBuilder erros = new StringBuilder();

                var body = await response.Content.ReadAsStringAsync();
                var validationProblemDetails = JsonSerializer.Deserialize<ValidationProblemDetails>(body);

                if (validationProblemDetails.Errors != null)
                {
                    foreach (var error in validationProblemDetails.Errors)
                    {
                        erros.AppendLine(error.Value.FirstOrDefault());
                    }
                }

                ErrorMessage = erros.ToString();
            }
        }

        public Blob()
        {
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
            blobFormDto = new BlobFormDto();
            var httpClient = new HttpClient();
            _blobService = new BlobService(new  HttpClient());
        }

        public Blob(string errorMessage, string successMessage)
        {
            ErrorMessage = errorMessage;
            SuccessMessage = successMessage;
            blobFormDto = new BlobFormDto();
            _blobService = new BlobService(new HttpClient());
        }
    }
}
