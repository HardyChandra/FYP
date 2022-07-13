using System.ComponentModel.DataAnnotations;

namespace FYP.Models.AdminModel
{
    public class ChangePassword
    {
        public string AdminID { get; set; }

        [Required(ErrorMessage = "Please enter your old password!")]
        [Display(Name = "Old Password")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,20}$", ErrorMessage = "Your password must be at least 8 characters long and contain upperletter, number, and special character")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "Please enter your new password!")]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,20}$", ErrorMessage = "Your password must be at least 8 characters long and contain upperletter, number, and special character")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Please enter your confirm new password!")]
        [Display(Name = "Confirm New Password")]
        [Compare(nameof(NewPassword), ErrorMessage = "new password and confirm new password do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; }
    }
}
