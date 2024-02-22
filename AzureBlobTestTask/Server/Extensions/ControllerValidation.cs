using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AzureBlobTestTask.Server.Extensions
{
    public static class ControllerValidation
    {
        public static void ValidateModel(this ControllerBase controller, object viewModel)
        {
            controller.ModelState.Clear();

            var validationContext = new ValidationContext(viewModel, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(viewModel, validationContext, validationResults, true);

            foreach (var result in validationResults)
            {
                foreach (var name in result.MemberNames)
                {
                    controller.ModelState.AddModelError(name, result.ErrorMessage);
                }
            }
        }
    }
}
