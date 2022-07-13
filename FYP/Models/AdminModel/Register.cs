using System;
using System.ComponentModel.DataAnnotations;

namespace FYP.Models.AdminModel
{
    public class Register
    {
        public string AdminID { get; set; }
        [Required(ErrorMessage = "Please enter your name!")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter your email!")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter your company name!")]
        [Display(Name = "Company Name")]
        public string Company { get; set; }
        [Required(ErrorMessage = "Please enter your password!")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,20}$", ErrorMessage = "Your password must be at least 8 characters long and contain upperletter, number, and special character")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please enter your confirmation password!")]
        [Display(Name = "Confirm Password")]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmationPassword { get; set; }
        [Required(ErrorMessage = "Please enter your phone number!")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Please enter your birth date!")]
        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        public DateTime DoB { get; set; }
    }
}
