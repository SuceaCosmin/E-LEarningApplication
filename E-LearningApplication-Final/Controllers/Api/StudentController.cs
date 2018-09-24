using AutoMapper;
using E_LearningApplication_Final.Dtos;
using E_LearningApplication_Final.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace E_LearningApplication_Final.Controllers.Api
{
    public class StudentController : ApiController
    {


        private ELearningDatabaseEntities _context;


        public StudentController()
        {
            _context = new ELearningDatabaseEntities();
        }

        //GET/api/students
        public IEnumerable<StudentDto> GetStudents()
        {
            var studentRole = _context.Roles.SingleOrDefault(c => c.Name == "Student");
            var studentList = _context.Users.Where(user => user.Roles.Any(r => r.Id == studentRole.Id)).ToList();

            
            if(_context.UserLogins.Count() > 0)
            {
                string id = "";
                var userLogins = _context.UserLogins.ToList();
                foreach(var usr in userLogins)
                {
                    id = usr.UserId;
                    var query = _context.Users.SingleOrDefault(uid => uid.Id == id);
                    studentList.Add(query);
                }
            }
            

            List<StudentDto> studDto = new List<StudentDto>();
            studentList.ForEach(user =>
            {

                StudentDto dto = Mapper.Map<StudentDto>(user);

                studDto.Add(dto);
            });

            return studDto;
        }

        //Get /api/students/1
        public IHttpActionResult GetStudent(string id)
        {
            var student = _context.Users.SingleOrDefault(c => c.Id == id);

            if (student == null)
                return NotFound();
            return Ok(Mapper.Map<User, StudentDto>(student));
            
        }

        //DELETE /api/students/1
        [HttpDelete]
        public IHttpActionResult DeleteStudent(string id)
        {
            var studentInDb = _context.Users.SingleOrDefault(c => c.Id == id);

            if (studentInDb == null)
                return NotFound();


            var subscriptions = _context.Subscriptions.Where(x => x.Student_Id == id).ToList();
            if (subscriptions != null)
            {
                _context.Subscriptions.RemoveRange(subscriptions);
                _context.SaveChanges();
            }

            _context.Users.Remove(studentInDb);
            _context.SaveChanges();

            return Ok();

        }
    }
}
