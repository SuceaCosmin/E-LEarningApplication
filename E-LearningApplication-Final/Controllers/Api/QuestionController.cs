using AutoMapper;
using E_LearningApplication_Final.Dtos;
using E_LearningApplication_Final.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using System.Web;

namespace E_LearningApplication_Final.Controllers.Api
{
    public class QuestionController : ApiController
    {
        private ELearningDatabaseEntities _context;


        public QuestionController()
        {
            _context = new ELearningDatabaseEntities();
        }

        //GET/api/questions
        public IEnumerable<QuestionDto> GetQuestions(string query = null)
        {
            var questionQuery = _context.Questions.Include(c => c.RightAnswer_Id).Include( c=> c.CourseID_Id);
            //MAGIC NUMBER
            var firstQuery = questionQuery.Where(c => c.CourseID_Id.Equals(1));
            var secondQuery = firstQuery.ToList().Select(Mapper.Map<Question, QuestionDto>);


            if (!String.IsNullOrWhiteSpace(query))
                questionQuery = questionQuery.Where(c => c.Body.Contains(query));


            return secondQuery;
        }

        //Get /api/question/1
        public IHttpActionResult GetQuestion(int id)
        {
            var question = _context.Questions.SingleOrDefault(c => c.Id == id);

            if (question == null)
                return NotFound();
            return Ok(Mapper.Map<Question, QuestionDto>(question));
        }

        //POST /api/question
        [HttpPost]
        public IHttpActionResult CreateQuestion(QuestionDto questionDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var question = Mapper.Map<QuestionDto, Question>(questionDto);

            _context.Questions.Add(question);
            _context.SaveChanges();

            questionDto.Id = question.Id;

            return Created(new Uri(Request.RequestUri + "/" + question.Id), questionDto);
        }


        //PUT /api/questions/1
        [HttpPut]
        public IHttpActionResult UpdateQuestion(int id, QuestionDto questionDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var questionInDb = _context.Questions.SingleOrDefault(c => c.Id == id);

            if (questionInDb == null)
                return NotFound();

            Mapper.Map(questionDto, questionInDb);


            _context.SaveChanges();
            return Ok();
        }


        //DELETE /api/question/1
        [HttpDelete]
        public IHttpActionResult DeleteQuestion(int id)
        {
            var questionInDb = _context.Questions.SingleOrDefault(c => c.Id == id);

            if (questionInDb == null)
                return NotFound();

            _context.Questions.Remove(questionInDb);
            _context.SaveChanges();

            return Ok();

        }
    }
}
