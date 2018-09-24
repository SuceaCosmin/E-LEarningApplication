using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_LearningApplication_Final.Models;
using E_LearningApplication_Final.Dtos;

namespace E_LearningApplication_Final.ViewModels
{
    public class SelectedQuestionsViewModel
    {
     
        public IEnumerable<Question> Question { get; set; }

        public int Course_id { get; set; }
    }
}