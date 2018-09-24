using AutoMapper;
using E_LearningApplication_Final.Dtos;

using E_LearningApplication_Final.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace E_LearningApplication_Final.Controllers
{
    public class QuestionController : Controller
    {
        private ELearningDatabaseEntities _context;
        int globalCourseID;

        public int getglobalCourseID()
        {
            return this.globalCourseID;
        }

        public void setglobalCourseID(int value)
        {
            this.globalCourseID = value;
        }
        public QuestionController()
        {
            _context = new ELearningDatabaseEntities();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }



        // GET: Question
        public ViewResult Index(int courseId)
        {
           
             var viewModel = new SelectedQuestionsViewModel();

      
            var firstQuery = _context.Questions.Where(c => c.CourseID_Id.Equals(courseId)).ToList();
            var correspondingCourse = _context.Courses.SingleOrDefault(c => c.Id == courseId);
           
            if (firstQuery.Count() == 0)
            {

                viewModel = new SelectedQuestionsViewModel
                {
                    Question = _context.Questions.Where(c => c.CourseID_Id.Equals(courseId)).ToList(),
                    Course_id = courseId
                };

            }
            else
            {
               
                viewModel = new SelectedQuestionsViewModel
                {
                    Question = _context.Questions.Where(c => c.CourseID_Id.Equals(courseId)).ToList(),
                    Course_id = courseId,
                };
            }



            return View("Index", viewModel);
        }

        public ActionResult Edit(int id)
        {
            setglobalCourseID(id);
            var choices = _context.Choices.ToList();
            var question = _context.Questions.SingleOrDefault(c => c.Id == id);

            if (question == null)
                return HttpNotFound();

            var viewModel = new QuestionFormViewModel
            {
                Question = question,
                Choice = choices,
                Course_id = id
            };

            return View("QuestionForm", viewModel);

        }

        public ActionResult Delete(int id)
        {
            var question = _context.Questions.SingleOrDefault(c => c.Id == id);
            var questionid = question.CourseID_Id;
            _context.Questions.Remove(question);
            _context.SaveChanges();

             var viewModel = new SelectedQuestionsViewModel
            {
                Question = _context.Questions.Where(c => c.CourseID_Id.Equals(questionid)).ToList(),
                Course_id = questionid,
            };

            return View("Index",viewModel);
        }

        public ActionResult NewQuestion(int id)
        {
            setglobalCourseID(id);
            var choices = _context.Choices.ToList();
            var correspondingCourse = _context.Courses.SingleOrDefault(c => c.Id == id);
            var viewModel = new QuestionFormViewModel
            {
                Question = new Question { CourseID_Id = id } ,
                Choice = choices,
                Course_id = id
            };
            return View("QuestionForm", viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(QuestionFormViewModel viewModel)
        {


            if(!ModelState.IsValid)
            {
               return View("QuestionForm", viewModel);
            }





            if (viewModel.Question.Id == 0)
            {
                viewModel.Question.CourseID_Id = viewModel.Course_id;
                _context.Questions.Add(viewModel.Question);
            }
            else
            {
                var questionInDb = _context.Questions.Single(c => c.Id == viewModel.Question.Id);
                questionInDb.Body = viewModel.Question.Body;
                questionInDb.AnswerA = viewModel.Question.AnswerA;
                questionInDb.AnswerB = viewModel.Question.AnswerB;
                questionInDb.AnswerC = viewModel.Question.AnswerC;
                questionInDb.AnswerD = viewModel.Question.AnswerD;
                questionInDb.RightAnswer_Id = viewModel.Question.RightAnswer_Id;
                questionInDb.CourseID_Id = viewModel.Question.CourseID_Id;
            }


            _context.SaveChanges();

            var firstQuery = _context.Questions.Where(c => c.CourseID_Id.Equals(viewModel.Question.CourseID_Id)).ToList();
            var questionsViewModel = new SelectedQuestionsViewModel
            {
                Question = firstQuery,
                Course_id = viewModel.Question.CourseID_Id,
            };

            return View("Index", questionsViewModel);

        }



    }
}