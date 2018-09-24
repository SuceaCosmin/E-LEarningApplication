using E_LearningApplication_Final.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_LearningApplication_Final.ViewModels
{
    public class TestViewModel
    {
        public string TestName { get; set; }
        public int CourseId { get; set; }

        public List<QuestionJavascriptModel> QuestionList { get; set; }
    }
}