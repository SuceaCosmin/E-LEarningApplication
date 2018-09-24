using E_LearningApplication_Final.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_LearningApplication_Final.ViewModels
{
    public class CourseFormViewModel
    {
        public Cours Course { get; set; }
        public IEnumerable<User> Professor { get; set; }

        public float RateTestPast { get; set; }
        public int NrOfRegisteredStudents { get; set; }
        
        public bool Subscribed { get; set; }
    }
}