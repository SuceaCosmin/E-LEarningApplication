using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_LearningApplication_Final.Models
{
    public class Subscription
    {
        public int id  { get;set; }
        public User Student { get; set; }
        public Cours Course { get; set; }
    }
}