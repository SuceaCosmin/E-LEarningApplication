using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_LearningApplication_Final.Dtos
{
    public class ProfessorDto
    {
        public string Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Email { get; set; }
        public byte[] UserPhoto { get; set; }
        public string FullName { get; set; }
        public byte[] CV { get; set; }
    }
}