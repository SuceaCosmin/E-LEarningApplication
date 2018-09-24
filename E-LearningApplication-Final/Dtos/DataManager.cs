using E_LearningApplication_Final.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_LearningApplication_Final.Dtos
{
    public class DataManager
    {
        private ELearningDatabaseEntities _context;


        public DataManager() {
            _context = new ELearningDatabaseEntities();
        }

        #region Student

        public List<Cours> GetUserCoursers(string userId) {

            var userSubscriptionList = _context.Subscriptions.Where(subscription => subscription.Student_Id.Equals(userId) ).ToList();
            var courseList = _context.Courses.ToList();


            List<Cours> userCourseList = new List<Cours>();
            userSubscriptionList.ForEach(subscription =>
            {
                Cours course = courseList.Find(c => c.Id.Equals(subscription.Course_Id));
                if (course != null) {
                    courseList.Add(course);
                }
                
            });

            return courseList;
          
        }
        #endregion
    }
}