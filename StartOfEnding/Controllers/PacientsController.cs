using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClassLibraryDbContext;
using Models.Model;
using StartOfEnding.ModelsDTO;

namespace StartOfEnding.Controllers
{
    public class PacientsController : Controller
    {
        private readonly SqliteContext _context;

        public PacientsController(SqliteContext context)
        {
            _context = context;
        }

        // GET: Pacients
        public async Task<IActionResult> Index()
        {
            var sqliteContext = _context.Pacients.Include(p => p.Objective);
            return View(await sqliteContext.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Pacient pacient)
        {
            if (ModelState.IsValid)
            {
                pacient.AddDay = DateTime.Now;
                _context.Pacients.Add(pacient);
                await _context.SaveChangesAsync();
                ViewBag.idPacient = pacient.Id;
                ViewBag.Data = pacient.Name + " " + pacient.Surname;
                return View("Objective");
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddObjective(int id, ObjectiveDTO objDTO)
        {
            var pcnt = await _context.Pacients.FirstOrDefaultAsync(x => x.Id == id);
            Objective obj = new Objective
            {
                Height = objDTO.Height,
                IMT = objDTO.IMT,
                Weight = objDTO.Weight,
                Pacient = pcnt
            };
            _context.Objectives.Add(obj);
            _context.SaveChanges();
            pcnt.Objective = obj;
            _context.SaveChanges();
            return RedirectToAction("Pacients", "Admin");
        }
    }
}
