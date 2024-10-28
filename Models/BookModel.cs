using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core_MVC.Models
{
    public class BookModel
    {
        [Key]
        public int b_id { get; set; }

        [Required(ErrorMessage ="Enter Book Name")]
        [Display(Name ="Book Name : ")]
        public string? b_name { get; set; }

        [Required(ErrorMessage ="Enter Author Name")]
        [Display(Name ="Author Name : ")]
        public string? b_author { get; set; }

        [Required(ErrorMessage ="*")]
        [Range(1,1000,ErrorMessage ="Enter Price Between 1 To 1000")]
        [Display(Name ="Book Price")]
        public float b_price { get; set; }
    }
}