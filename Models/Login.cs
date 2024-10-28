using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Core_MVC.Models
{
    public class Login
    {
        [Display(Name = "UserName")]
        [Required(ErrorMessage ="Enter Username")]
        public string c_username { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage ="Enter Password")]
        public string c_password { get; set; }
    }
}