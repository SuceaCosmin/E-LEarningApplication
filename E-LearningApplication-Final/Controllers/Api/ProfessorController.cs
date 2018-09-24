using AutoMapper;
using E_LearningApplication_Final.Dtos;
using E_LearningApplication_Final.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace E_LearningApplication_Final.Controllers.Api
{
    public class ProfessorController : ApiController
    {
        private ELearningDatabaseEntities _context;

        public ProfessorController()
        {
            _context = new ELearningDatabaseEntities();
        }

        //GET/api/professors
        public IEnumerable<ProfessorDto> GetProfessors()
        {
            var professorRole = _context
                .Roles
                .SingleOrDefault(c => c.Name == "Professor");
    
    
            var professorList = _context
                .Users.Where(m => m.Roles.Any(r => r.Id == professorRole.Id))
                .ToList();


            List<ProfessorDto> profDto = new List<ProfessorDto>();
            professorList.ForEach(user =>
            {

                ProfessorDto dto = Mapper.Map<ProfessorDto>(user);

                profDto.Add(dto);
            });

            return profDto;//getProfessors;
        }

        //DELETE /api/professors/1
        [HttpDelete]
        public IHttpActionResult DeleteProfessor(string id)
        {
            var professorInDb = _context.Users.SingleOrDefault(c => c.Id == id);

            if (professorInDb == null)
               return NotFound();

            _context.Users.Remove(professorInDb);
            _context.SaveChanges();

            return Ok();

        }
    }
}
