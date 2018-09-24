using AutoMapper;
using E_LearningApplication_Final.Dtos;
using E_LearningApplication_Final.Models;
using E_LearningApplication_Final.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_LearningApplication_Final.Controllers
{
    public class ProfessorController : Controller
    {

        private ELearningDatabaseEntities _context;

        public ProfessorController()
        {
            _context = new ELearningDatabaseEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Professor
        public ActionResult Index()
        {
            if(User.IsInRole("Professor") || (User.IsInRole("Admin")))
            {
                return View("Index");
            }
              
            else
            {
                return View("IndexForStudentsView");
            }
                 
        }
        public ActionResult Edit(string id)
        {
            var professor = _context.Users.SingleOrDefault(c => c.Id == id);

            if (professor == null)
                return HttpNotFound();

            var model = new User { Email = professor.Email, CV = professor.CV, UserPhoto = professor.UserPhoto, FullName = professor.FullName };
            return View("ProfessorForm", model);
        }

        public ActionResult Details(string id)
        {
            var professor = _context.Users.SingleOrDefault(c => c.Id == id);

            if (professor == null)
                return HttpNotFound();

            var firstQuery = _context.Users.Single(c => c.Id == id);
            List<CourseDto> dtoList = new List<CourseDto>();
            var courses = _context.Courses.Where(c => c.Author_Id == firstQuery.Id).ToList();

            courses.ForEach(course =>
            {
                CourseDto dto = Mapper.Map<CourseDto>(course);
                dtoList.Add(dto);
            });

            var viewModel = new ProfessorDetails
            {
                Professor = Mapper.Map<User, ProfessorDto>(professor),
                ProfessorCourses = dtoList
            };

            return View("Details", viewModel);
        }

        public void ViewCV(string id)
        {
            var professor = _context.Users.SingleOrDefault(c => c.Id == id);
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-length", professor.CV.Length.ToString());
            Response.BinaryWrite(professor.CV);
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(User professor, HttpPostedFileBase file, HttpPostedFileBase cv)

        {
            var professorInDb = _context.Users.Single(c => c.Id == professor.Id);

            professorInDb.FullName = professor.FullName;
            professorInDb.Email = professor.Email;

           
            if(file!= null)
            {
                byte[] imageData = null;
                using (var binary = new BinaryReader(file.InputStream))
                {
                    imageData = binary.ReadBytes(file.ContentLength);
                    professor.UserPhoto = imageData;
                }
            }
               
            if(cv!= null)
            {
                byte[] cvPdf = null;
                using (var binary = new BinaryReader(cv.InputStream))
                {
                    cvPdf = binary.ReadBytes(cv.ContentLength);
                    professor.CV = cvPdf;
                }
            }
               


            

            _context.SaveChanges();
            return RedirectToAction("Index", "Professor");

        }
        [HttpDelete]
        public ActionResult DeleteProfessor(string id)
        {
            var professorInDb = _context.Users.SingleOrDefault(c => c.Id == id);


            _context.Users.Remove(professorInDb);
            _context.Courses.RemoveRange(_context.Courses.Where(c => c.Author_Id == id));
            _context.SaveChanges();

            return View("Index");

        }


    }
}