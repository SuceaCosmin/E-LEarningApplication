using AutoMapper;
using E_LearningApplication_Final.Dtos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_LearningApplication_Final.App_Start
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<User, ProfessorDto>();
            Mapper.CreateMap<ProfessorDto, User>();

            Mapper.CreateMap<User, StudentDto>();
            Mapper.CreateMap<StudentDto, User>();

            Mapper.CreateMap<Cours, CourseDto>();
            Mapper.CreateMap<CourseDto, Cours>();

           
            Mapper.CreateMap<Question, QuestionDto>();
            Mapper.CreateMap<QuestionDto, Question>();

            //Mapper.CreateMap<Test, TestDto>();
            //Mapper.CreateMap<TestDto, Test>();


        }
    }
}