using Microsoft.AspNetCore.Mvc.ModelBinding;
using OneRegister.Core.Model.ControllerResponse;
using System.Linq;
using System.Text;

namespace OneRegister.Framework.Extensions
{
    public static class ModelStateExtensions
    {
        public static FullResponse FullResponse(this ModelStateDictionary modelState)
        {
            var fullResponse = new FullResponse { IsSuccessful = false };
            foreach (var key in modelState.Keys)
            {
                if (modelState[key].ValidationState == ModelValidationState.Invalid)
                {
                    fullResponse.Validations.Add(new ValidationModel
                    {
                        Field = key,
                        Description = modelState[key].Errors.FirstOrDefault().ErrorMessage
                    });
                }
            }
            return fullResponse;
        }

        public static string ConcatToString(this ModelStateDictionary modelState)
        {
            var text = new StringBuilder();
            foreach (var key in modelState.Keys)
            {
                if (modelState[key].ValidationState == ModelValidationState.Invalid)
                {
                    text.Append($"*{key}: {modelState[key].Errors.FirstOrDefault().ErrorMessage} ");
                }
            }
            return text.ToString();
        }
    }
}