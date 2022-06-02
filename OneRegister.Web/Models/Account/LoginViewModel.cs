using System.ComponentModel.DataAnnotations;

namespace OneRegister.Web.Models.Account
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
    }
}
