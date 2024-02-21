using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FunctionApp
{
    public class EmailNotificationFunction
    {

        [Function(nameof(EmailNotificationFunction))]
        public void Run([BlobTrigger("tesktask/{name}", Connection = "AzureWebJobsStorage")] Stream stream, string name,
            IDictionary<string, string> metaData)
        {
            
        }
    }
}
