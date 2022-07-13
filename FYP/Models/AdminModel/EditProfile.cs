using System;
using System.ComponentModel.DataAnnotations;

namespace FYP.Models.AdminModel
{
	public class EditProfile
	{
        public string admin_id { get; set; }
        [Required(ErrorMessage = "Please enter your name!")]
        [Display(Name = "Name")]
        public string admin_name { get; set; }
        [Required(ErrorMessage = "Please enter your email!")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string admin_email { get; set; }
        [Required(ErrorMessage = "Please enter your company name!")]
        [Display(Name = "Company Name")]
        public string admin_company { get; set; }
        [Required(ErrorMessage = "Please enter your phone number!")]
        [Display(Name = "Phone Number")]
        public string admin_phone { get; set; }
        [Required(ErrorMessage = "Please enter your birth date!")]
        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        public DateTime admin_dob { get; set; }
    }
}
