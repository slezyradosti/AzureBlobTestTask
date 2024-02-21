using System.ComponentModel.DataAnnotations;

namespace AzureBlobTestTask.Server.Extensions
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FileExtensionsAttribute : ValidationAttribute
    {
        private List<string> AllowedExtensions { get; set; }

        public FileExtensionsAttribute(string fileExtensions)
        {
            AllowedExtensions = fileExtensions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        /// <summary>
        /// check file for '.fileExtension'
        /// eg: '.png,.jpg'
        /// </summary>
        public override bool IsValid(object value)
        {
            IFormFile file = value as IFormFile;

            if (file != null)
            {
                return AllowedExtensions.Any(x => Path.GetExtension(file.FileName) == x);
            }

            return true;
        }
    }
}
