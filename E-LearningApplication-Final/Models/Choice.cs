using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_LearningApplication_Final.Models
{
    public class Choice
    {
        public byte Id { get; set; }
        [Required]
        public string Name { get; set; }


    }
}