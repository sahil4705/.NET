using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core_MVC.Models
{
    public class User
    {
        public int c_userid { get; set; }

        [Display(Name = "UserName")]
        [Required(ErrorMessage ="Enter Username")]
        public string c_username { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage ="Enter Email Address")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",ErrorMessage ="Please enter a valid email address")]
        public string c_email { get; set; }

        [Display(Name ="Gender")]
        // [Required(ErrorMessage ="Select Your Gender")]
        public string c_gender { get; set; }

        [Display(Name ="Password")]
        [Required(ErrorMessage ="Enter Password")]
        public string c_password { get; set; }

        [Display(Name ="Conform Password")]
        [Required(ErrorMessage ="Enter Conform Password")]
        
        [Compare("c_password", ErrorMessage = "The password and confirmation password do not match.")]
        public string c_conform_password { get; set; }
        // public string VerificationToken { get; internal set; }
        // public bool IsVerified { get; internal set; }
    }
}