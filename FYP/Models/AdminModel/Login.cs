using System.ComponentModel.DataAnnotations;

namespace FYP.Models.AdminModel
{
    public class Login
    {
        [Required(ErrorMessage = "Please enter your email!")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter your password!")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
