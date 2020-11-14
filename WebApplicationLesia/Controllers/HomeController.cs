using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClassLibraryDbContext;
using LesiaWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Model;
//using WebApplicationLesia.Models;

namespace WebApplicationLesia.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly SqliteContext context;
        private readonly IMapper mapper;

        public HomeController(ILogger<HomeController> _logger, SqliteContext _context, IMapper _mapper)
        {
            logger = _logger;
            context = _context;
            mapper = _mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var pacients = context.Pacients.ToList();
            var pacientsDTO = mapper.Map<IEnumerable<Pacient>,
                                         IEnumerable<PacientDTO>>(pacients);
            return View(pacientsDTO);
        }
        //[HttpGet]
        //public async Task<ActionResult<List<PacientDTO>>> GetAll()
        //{
        //    var pacients = await context.Pacients
        //        .Include(x => x.Complaints)
        //        .Include(z => z.PacientDiagnoses)
        //        .Include(y => y.Objective)
        //        .ToListAsync();

        //    var pacientsDTO = mapper.Map<IEnumerable<Pacient>,
        //                                 IEnumerable<PacientDTO>>(pacients);
        //    return View(pacientsDTO);
        //}
        [HttpPost] //create
        public async Task<ActionResult<Pacient>> AddPacient(PacientDTO pacientDTO)
        {
            if (pacientDTO == null)
            {
                return BadRequest();
            }
            var pacient = mapper.Map<PacientDTO, Pacient>(pacientDTO);
            context.Add(pacient);
            await context.SaveChangesAsync();
            return Ok(pacientDTO);
        }
    }
}
