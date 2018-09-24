using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_LearningApplication_Final.ViewModels
{
    public class CourseReferenceViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public string Author_Id { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public byte[] Content { get; set; }
        [Required]
        public byte[] CoverPohoto { get; set; }

        public string Rate { get; set; }
    }
}