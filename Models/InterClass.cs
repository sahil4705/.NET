using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core_MVC.Models
{
    public class InterClass : AssignTaskClass
    {
        [Display(Name ="InternID")] 
        public int c_internid1 { get; set; }

        [Display(Name ="Intern Name")]
        [StringLength(100, ErrorMessage = "Intern Name can't exceed 100 characters")]
        [Required(ErrorMessage = "Intern Name is required.")]
        public string? c_InternName { get; set; }

        [Display(Name ="Intern Gender")]
        [Required(ErrorMessage = "Intern Gender is required.")]
        public char c_Gender { get; set; }

        [Display(Name ="Intern Topic Name")]
        [Required(ErrorMessage = "Intern Name is required.")]
        public int c_TopicId { get; set; }

        [Display(Name ="Intern Presentation Date")]
         [DataType(DataType.Date)]
        [Required(ErrorMessage = "Intern Name is required.")]
        public DateTime c_Date_Of_Presentation { get; set; }

        [Display(Name ="Intern Status")]
        [Required(ErrorMessage = "Intern Status is required.")]
        public bool c_Status { get; set; }

        [Display(Name ="Intern Image")]
       // [Required(ErrorMessage = "Intern Image is required.")]
        public string? c_Topic_Image { get; set; }
       
    }
}