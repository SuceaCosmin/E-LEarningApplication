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
    public class TestController : ApiController
    {
        private ELearningDatabaseEntities _context;
        
        public TestController()
        {
            _context = new ELearningDatabaseEntities();
        }

       

    }
}
