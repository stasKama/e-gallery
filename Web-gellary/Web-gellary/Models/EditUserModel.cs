using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web_gellary.Models
{
    public class EditUserModel
    {
        public UpdatePassword UserPassword { get; set; }
        public Users User { get; set; }
    }

    public class UpdatePassword
    {
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 30 characters")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Plese confirm your password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}