using AutoMapper;
using E_LearningApplication_Final.Dtos;
using E_LearningApplication_Final.Models;
using E_LearningApplication_Final.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace E_LearningApplication_Final.Controllers
{
    public class StudentController : Controller
    {
        private ELearningDatabaseEntities _context;

        public StudentController()
        {
            _context = new ELearningDatabaseEntities();
        }


        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        public ActionResult Index()
        {
           return View();
        }

        public ActionResult Delete(string id)
        {
            var studentInDb = _context.Users.SingleOrDefault(c => c.Id == id);

            _context.Users.Remove(studentInDb);
            _context.Subscriptions.RemoveRange(_context.Subscriptions.Where(c => c.Student_Id == id));
            _context.SaveChanges();

            return View("Index");

        }

        public ActionResult Details(string id)
        {
            var student = _context.Users.SingleOrDefault(c => c.Id == id);

            if (student == null)
                return HttpNotFound();

            var userSubscriptionList = _context.Subscriptions.Where(subscription => subscription.Student_Id.Equals(student.Id)).ToList();

            var userList = _context.Users;
            List<CourseDto> dtoList = new List<CourseDto>();
            userSubscriptionList.ForEach(course => {

                User user = userList.SingleOrDefault(u => u.Id.Equals(course.Cours.Author_Id));
                CourseDto dto = Mapper.Map<CourseDto>(course.Cours);
                dto.Author_Id = user.Email;
                dtoList.Add(dto);

            });

            var viewModel = new StudentDetails
            {
                Student = Mapper.Map<User, StudentDto>(student),
                StudentCourses = dtoList



            };


            return View(viewModel);
           
        }

       
    }
}