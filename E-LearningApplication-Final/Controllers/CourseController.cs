using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using E_LearningApplication_Final.ViewModels;
using System.IO;
using AutoMapper;
using E_LearningApplication_Final.Dtos;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using E_LearningApplication_Final.Models;

namespace E_LearningApplication_Final.Controllers
{
    public class CourseController : Controller
    {

        private DataManager _dataManager;
        private ELearningDatabaseEntities _context;

        public CourseController()
        {
            _dataManager = new DataManager();
            _context = new ELearningDatabaseEntities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        public ViewResult Index()
        {
            return View();
        }

        public ViewResult ListOfCourses()
        {
            return View();
        }

        #region Student
        public ActionResult StudentCourses()
        {
            string userIdentity = User.Identity.GetUserId();
            var userSubscriptionList = _context.Subscriptions.Where(subscription => subscription.Student_Id.Equals(userIdentity)).ToList();
            var userList = _context.Users;
            List<CourseDto> dtoList = new List<CourseDto>();

            userSubscriptionList.ForEach(course => {
                CourseDto dto = Mapper.Map<CourseDto>(course.Cours);
                dtoList.Add(dto);
            });
            return View("StudentCourses",dtoList);

        }

        public ActionResult TestsResultsStudent()
        {
            string userIdentity = User.Identity.GetUserId();
            var firstQuery = _context.Tests.Where(c => c.Student_ID.Equals(userIdentity)).ToList();

            return View("TestsResults", firstQuery);

        }

        public ViewResult Subscribe(int id)
        {
            var course = _context.Courses.SingleOrDefault(c => c.Id == id);
            var viewmodel = new CourseFormViewModel
            {
                Course = course,
                Professor = _context.Users.ToList()
            };
            var studentId = User.Identity.GetUserId();
            var firstQuery = _context.Users.Single(c => c.Id == studentId);
           
            Subscription newSubscription = new Subscription()
            {
                Student_Id = firstQuery.Id,
                Course_Id = course.Id
            };
            _context.Subscriptions.Add(newSubscription);
            _context.SaveChanges();
            return View("Details", viewmodel);
        }

        public ActionResult UnSubscribe(int id)
        {
            _context.Subscriptions.RemoveRange(_context.Subscriptions.Where(c => c.Course_Id == id));
            _context.SaveChanges();


            string userIdentity = User.Identity.GetUserId();

            var userSubscriptionList = _context.Subscriptions.Where(subscription => subscription.Student_Id.Equals(userIdentity)).ToList();

            var userList = _context.Users;
            List<CourseDto> dtoList = new List<CourseDto>();
            userSubscriptionList.ForEach(course => {

                User user = userList.SingleOrDefault(u => u.Id.Equals(course.Cours.Author_Id));
                CourseDto dto = Mapper.Map<CourseDto>(course.Cours);
                dto.Author_Id = user.Email;
                dtoList.Add(dto);

            });
            return View("StudentCourses",dtoList);
        }

        public ActionResult TakeTest(int id)
        {
            var firstQuery = _context.Questions.Where(c => c.CourseID_Id == id);
            var secondQuery = firstQuery.ToList();
            var rnd = new Random();
            var randomTest = secondQuery.OrderBy(x => rnd.Next()).Take(5);

            List<QuestionJavascriptModel> questionModelList = new List<QuestionJavascriptModel>();

            foreach (var question in randomTest) {
                QuestionJavascriptModel questionModel = new QuestionJavascriptModel();
                questionModel.Q = question.Body;
                questionModel.A = question.RightAnswer_Id;

                List<string> answerList = new List<string>();
                answerList.Add(question.AnswerA);
                answerList.Add(question.AnswerB);
                answerList.Add(question.AnswerC);
                answerList.Add(question.AnswerD);
                questionModel.C = answerList;
                questionModelList.Add(questionModel);
            }

            TestViewModel model = new TestViewModel() {
                CourseId= id,
                QuestionList= questionModelList
            };
                  
            return View("Test", model);
        }

        public ActionResult SubmitTestResult(int courseId, int result)
        {
            string studentID = User.Identity.GetUserId();
            string Test_Name = User.Identity.Name + "_" + courseId;
            int testID = _context.Tests.Count() +1;
            DateTime dateTime = DateTime.UtcNow.Date;
            var hourTest = DateTime.Now.ToString("h:mm:ss tt");
            string testDate = dateTime.ToString("dd/MM/yyyy") + " " + hourTest;
            Test test = new Test { Id= testID, Course_ID = courseId, Test_Name = Test_Name, Student_ID = studentID, Result = result , TestDate = testDate };
            
            _context.Tests.Add(test);
            _context.SaveChanges();
            return   RedirectToAction("ListOfCourses", "Course");
        }

        public ActionResult SubscribedStudents(int id)
        {
            
            var userSubscriptionList = _context.Subscriptions.Where(subscription => subscription.Course_Id == id).ToList();
            var courseList = _context.Courses;
            List<StudentDto> dtoList = new List<StudentDto>();
            userSubscriptionList.ForEach(student =>
           {
               Cours course = courseList.Single(c => c.Id.Equals(id));
               StudentDto dto = Mapper.Map<StudentDto>(student.User);
               dtoList.Add(dto);

           });
            return View("SubscribedStudents", dtoList);

        }

        #endregion

        #region Professor

        /// <summary>
        /// 
        /// This method populates the view with the students results for a specific course
        /// </summary>
        /// <param name="id"> id of specific course</param>
        /// <returns></returns>
        public ActionResult StudentsTestResult(int id)
        {
            var firstQuery = _context.Tests.Where(c => c.Course_ID.Equals(id)).ToList();

            return View("StudentsTestResult", firstQuery); 
        }

        public ViewResult ProfessorCourses()
        {
            string userIdentity = User.Identity.GetUserId();
            var firstQuery = _context.Users.Single(c => c.Id == userIdentity);
            List<CourseDto> dtoList = new List<CourseDto>();
            var courses = _context.Courses.Where(c => c.Author_Id == firstQuery.Id).ToList();

            courses.ForEach(course =>
            {
                CourseDto dto = Mapper.Map<CourseDto>(course);
                dtoList.Add(dto);
            });
            return View(dtoList);
        }

        public ActionResult AddQuestions(int id)
        {
            return RedirectToAction("Index", "Question", new { courseId = id });
        }

        public ActionResult ProfessorDetails(int id)
        {
            var course = _context.Courses.SingleOrDefault(c => c.Id == id);
            var subscribedStudents = getNumberOfRegisteredStudents(id);
            var resultTest = getTestResults(id); 
            if (course == null)
                return HttpNotFound();


            var viewmodel = new CourseFormViewModel
            {
                Course = course,
                Professor = _context.Users.ToList(),
                NrOfRegisteredStudents = subscribedStudents,
                RateTestPast = resultTest


            };
            if(User.IsInRole("Professor") || User.IsInRole("Admin"))
            {
                return View("Edit", viewmodel);
            }
            else
            {
                return View("Details", viewmodel);
            }
         
        }

        public ActionResult Edit(int id)
        {

            var course = _context.Courses.SingleOrDefault(c => c.Id == id);

            if (course == null)
                return HttpNotFound();
      
            var viewmodel = new CourseFormViewModel
            {
                Course = course,
                Professor = _context.Users.ToList()


            };
            string userIdentity = User.Identity.GetUserId();
            var firstQuery = _context.Users.Single(c => c.Id == userIdentity);
            viewmodel.Course.Author_Id = firstQuery.Id;
            return View("CourseForm", viewmodel);
        }


        #endregion


        #region General

        public ActionResult SingleCourse()
        {
            return View("Details");
        }

        public ActionResult Delete(int id)
        {
            _context.Courses.RemoveRange(_context.Courses.Where(c => c.Id == id));
            _context.Questions.RemoveRange(_context.Questions.Where(c => c.CourseID_Id == id));
            _context.Tests.RemoveRange(_context.Tests.Where(c => c.Course_ID == id));
            _context.SaveChanges();

            if(User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Course");
            }
            else
                return RedirectToAction("ProfessorCourses", "Course");

        }

        public ActionResult Details(int id)
        {
            var course = _context.Courses.SingleOrDefault(c => c.Id == id);
            var registeredStudents = getNumberOfRegisteredStudents(id);
            var testresult = getTestResults(id);
            var subscription = checkSubscription(id);
            if (course == null)
                return HttpNotFound();
            var viewmodel = new CourseFormViewModel
            {
                Course = course,
                Professor = _context.Users.ToList(),
                NrOfRegisteredStudents = registeredStudents,
                RateTestPast = testresult,
                Subscribed = subscription
                


            };
            //string userIdentity = User.Identity.GetUserId();
            //var firstQuery = _context.Users.Single(c => c.Id == userIdentity);
            //viewmodel.Course.Author_Id = firstQuery.Id;
           
            return View("Details", viewmodel);
        }

        private bool checkSubscription(int id)
        {
            if(User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var userSubscriptions = _context.Subscriptions.Where(usr => usr.Student_Id == userId).ToList();
                foreach( var subscr in userSubscriptions)
                {
                    if(subscr.Course_Id.Equals(id))
                    {
                        return true;
                    }
                }

            }
            return false;
        }

        public ActionResult NewCourse()
        {

            var viewModel = new CourseFormViewModel
            {
                Course = new Cours(),
                Professor = _context.Users.ToList()


            };
            string userIdentity = User.Identity.GetUserId();
            var firstQuery = _context.Users.Single(c => c.Id == userIdentity);
            viewModel.Course.Author_Id = firstQuery.Id;
            return View("CourseForm", viewModel);
        }
        public void DownloadCourse(int id)
        {
            var course = _context.Courses.SingleOrDefault(c => c.Id == id);
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-length", course.Content.Length.ToString());
            Response.BinaryWrite(course.Content);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Cours course)
        {
            string userIdentity = User.Identity.GetUserId();
            var firstQuery = _context.Users.Single(c => c.Id == userIdentity);

            if (course.Id == 0)
            {
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase poImgFile = Request.Files["CoverPhoto"];
                    byte[] imageData = null;
                    using (var binary = new BinaryReader(poImgFile.InputStream))
                    {
                        imageData = binary.ReadBytes(poImgFile.ContentLength);
                    }
                    course.CoverPohoto = imageData;
                }

                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase PdfFile = Request.Files["Content"];
                    byte[] content = null;
                    using (var binary = new BinaryReader(PdfFile.InputStream))
                    {
                        content = binary.ReadBytes(PdfFile.ContentLength);
                    }
                    course.Content = content;
                }

               
                course.Author_Id = firstQuery.Id;
            
           
                _context.Courses.Add(course);
            }
            else
            {
                var courseInDb = _context.Courses.Single(c => c.Id == course.Id);
                courseInDb.Name = course.Name;
                courseInDb.Description = course.Description;
             
                //courseAuthor
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase PdfFile = Request.Files["Content"];
                    byte[] content = null;
                    using (var binary = new BinaryReader(PdfFile.InputStream))
                    {
                        content = binary.ReadBytes(PdfFile.ContentLength);
                    }
                    courseInDb.Content = content;
                }

                else
                    courseInDb.Content = course.Content;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase poImgFile = Request.Files["CoverPhoto"];
                    byte[] imageData = null;
                    using (var binary = new BinaryReader(poImgFile.InputStream))
                    {
                        imageData = binary.ReadBytes(poImgFile.ContentLength);
                    }
                    courseInDb.CoverPohoto = imageData;
                }
                else
                    courseInDb.CoverPohoto = course.CoverPohoto;



                
            }

            _context.SaveChanges();


         
            List<CourseDto> dtoList = new List<CourseDto>();
            var courses = _context.Courses.Where(c => c.Author_Id == firstQuery.Id).ToList();

            courses.ForEach(c =>
            {
                CourseDto dto = Mapper.Map<CourseDto>(c);
                dtoList.Add(dto);
            });

            if (User.IsInRole("Professor"))
            {
                return View("ProfessorCourses", dtoList);
            }
            else
                return View("Index");
           
        }

        #endregion


        #region Helpers
        private int getNumberOfRegisteredStudents(int id)
        {
            var userSubscriptionList = _context.Subscriptions.Where(subscription => subscription.Course_Id == id).ToList();
            var courseList = _context.Courses;
            List<StudentDto> dtoList = new List<StudentDto>();
            userSubscriptionList.ForEach(student =>
            {
                Cours course = courseList.Single(c => c.Id.Equals(id));
                StudentDto dto = Mapper.Map<StudentDto>(student.User);
                dtoList.Add(dto);

            });
             return dtoList.Count;
        }



        private int getTestResults(int id)
        {
            int result = 0;
            var userTests = _context.Tests.Where(test => test.Course_ID == id).ToList();
            if (userTests.Count() > 0)
            {
                foreach (var test in userTests)
                {
                    var r = test.Result * 100 / 5;
                    result = result + r;
                }
                return result / userTests.Count();

            }
            else
                return result;



        }
        #endregion



















    }
}