using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BaseProjectDataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ZVersion.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SqLiteContextUsers _context;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, IMapper mapper, SqLiteContextUsers context)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
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
