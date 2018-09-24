using E_LearningApplication_Final.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_LearningApplication_Final.ViewModels
{
    public class ProfessorDetails
    {
        public ProfessorDto Professor { get; set; }
        public List<CourseDto> ProfessorCourses { get; set; }
    }
}