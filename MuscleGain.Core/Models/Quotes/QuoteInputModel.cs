using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MuscleGain.Core.Models.Quotes
{
    public class QuoteInputModel
    {
        [Required]
        [Display(Name = "Content of the Quote")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Author Name must contain a minimum of 2 characters!")]
        [MaxLength(80, ErrorMessage = "Author Name maximum number of charachters is 80!")]
        [Display(Name = "Author of the Quote")]
        public string AuthorName { get; set; }

    }
}
