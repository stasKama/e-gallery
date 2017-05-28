using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web_gellary.Models
{
    public class UpdatePassword
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
                  ErrorMessageResourceName = "OldPasswordRequired")]
        [DataType(DataType.Password)]
        [Display(Name = "OldPassword", ResourceType = typeof(Resources.Resource))]
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
                  ErrorMessageResourceName = "NewPasswordRequired")]
        [DataType(DataType.Password)]
        [StringLength(30, MinimumLength = 8, ErrorMessageResourceName = "LengthPassword",
                  ErrorMessageResourceType = typeof(Resources.Resource))]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).{8,15}$", ErrorMessageResourceName = "ValidationPassword",
                  ErrorMessageResourceType = typeof(Resources.Resource))]
        [Display(Name = "NewPassword", ResourceType = typeof(Resources.Resource))]
        public string NewPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
                  ErrorMessageResourceName = "ConfirmPasswordRequired")]
        [Compare("NewPassword", ErrorMessageResourceName = "ComparePassword",
                  ErrorMessageResourceType = typeof(Resources.Resource))]
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Resources.Resource))]
        public string ConfirmPassword { get; set; }
    }
}