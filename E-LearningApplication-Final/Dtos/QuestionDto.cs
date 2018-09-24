using E_LearningApplication_Final.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_LearningApplication_Final.Dtos
{
    public class QuestionDto
    {
        public int Id { get; set; }
        [Required]
        public string Body { get; set; }
        [Required]
        public string AnswerA { get; set; }
        [Required]
        public string AnswerB { get; set; }
        [Required]
        public string AnswerC { get; set; }
        [Required]

        public string AnswerD { get; set; }
        [Required]

        public Choice RightAnswer { get; set; }

        public Cours CourseID { get; set; }
    }
}