using System.ComponentModel.DataAnnotations;

namespace OneRegister.Domain.Validation.Attributes
{
    public class CustomRequiredAttribute : RequiredAttribute
    {
        public CustomRequiredAttribute()
        {
            ErrorMessage = "Required";
        }
    }
}
