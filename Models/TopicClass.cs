using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core_MVC.Models
{
    public class TopicClass : InterClass
    {

        [Display(Name = "Intern TopicID")]
        public int c_TopicId { get; set; }
        
        [Display(Name = "Topic Name")]
        [Required(ErrorMessage = "Topic Name is required")]
        public string? c_TopicName { get; set; }
        
    }
}