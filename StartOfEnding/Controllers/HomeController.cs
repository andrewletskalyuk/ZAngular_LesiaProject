using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClassLibraryDbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.Model;
using StartOfEnding.Models;

namespace StartOfEnding.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SqliteContext _context;
        //private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, SqliteContext context)
        {
            _logger = logger;
            _context = context;
            //_mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AboutDoctor()
        {
            return View();
        }
    }
}
