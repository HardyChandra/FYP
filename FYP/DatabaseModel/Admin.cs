using System;
using System.ComponentModel.DataAnnotations;

namespace FYP.DatabaseModel
{
    public class Admin
    {
        public string admin_id { get; set; }
        public string admin_name { get; set; }
        public string admin_email { get; set; }
        public string admin_company { get; set; }
        public string admin_password { get; set; }
        public string admin_phone { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime admin_dob { get; set; }
        public string admin_role { get; set; }
    }
}
