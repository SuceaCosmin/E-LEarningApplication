using E_LearningApplication_Final;
using E_LearningApplication_Final.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_LearningApplication_Final.ViewModels
{
    public class QuestionFormViewModel
    {
        public IEnumerable<Choice> Choice { get; set; }
        public Question Question { get; set; }

        public int Course_id { get; set; }
    }
}