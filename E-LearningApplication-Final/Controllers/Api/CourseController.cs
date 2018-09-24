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
using Microsoft.AspNet.Identity;

namespace E_LearningApplication_Final.Controllers.Api
{
    public class CourseController : ApiController
    {
        private ELearningDatabaseEntities _context;


        public CourseController()
        {
            _context = new ELearningDatabaseEntities();
        }
        //GET/api/courses
        public IHttpActionResult GetCourses(string query = null)
        {
            //var courseQuery = _context.Courses.Include(c => c.Author_Id);
            var userList = _context.Users;

            List<CourseDto> dtoList = new List<CourseDto>();
            _context.Courses.ToList().ForEach(course=>{
                
                User user = userList.SingleOrDefault(u => u.Id.Equals(course.Author_Id));
                CourseDto dto = Mapper.Map<CourseDto>(course);
                dto.Author_Id = user.Email;
                dtoList.Add(dto);

            });

          

            return Ok(dtoList);
           
        }



        //Get /api/course/1
        public IHttpActionResult GetCourse(int id)
        {
            var course = _context.Courses.SingleOrDefault(c => c.Id == id);

            if (course == null)
                return NotFound();
            return Ok(Mapper.Map<CourseDto>(course));
        }

        //POST /api/courses
        [HttpPost]
        public IHttpActionResult CreateCourse(CourseDto courseDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            courseDto.Author_Id = User.Identity.GetUserId();
            var course = Mapper.Map<Cours>(courseDto);

            
            _context.Courses.Add(course);
            _context.SaveChanges();

            courseDto.Id = course.Id;

            return Created(new Uri(Request.RequestUri + "/" + course.Id), courseDto);
        }

        //PUT /api/courses/1
        [HttpPut]
        public void UpdateCourse(int id, CourseDto courseDto)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var courseInDb = _context.Courses.SingleOrDefault(c => c.Id == id);

            if (courseInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            Mapper.Map(courseDto, courseInDb);


            _context.SaveChanges();
        }

        //DELETE /api/courses/1
        [HttpDelete]
        public void DeleteCourse(int id)
        {
            var courseInDb = _context.Courses.SingleOrDefault(c => c.Id == id);

            if (courseInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            var subscriptions = _context.Subscriptions.Where(x => x.Course_Id == id).ToList();
            if(subscriptions != null)
            {
                _context.Subscriptions.RemoveRange(subscriptions);
                _context.SaveChanges();
            }

            var questions = _context.Questions.Where(x => x.CourseID_Id == id).ToList();
            if(questions != null)
            {
                _context.Questions.RemoveRange(questions);
                _context.SaveChanges();
            }

            var tests = _context.Tests.Where(x => x.Course_ID == id).ToList();
            if(tests != null)
            {
                _context.Tests.RemoveRange(tests);
                _context.SaveChanges();
            }
            _context.Courses.Remove(courseInDb);
            _context.SaveChanges();

        }

       

    }
}
