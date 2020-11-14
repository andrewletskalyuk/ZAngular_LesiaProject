using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibraryDbContext;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Models.Model;
using Models.Models;
using StartOfEnding.ModelsDTO;
using StartOfEnding.Services;

namespace StartOfEnding.Controllers
{
    public class AdminController : Controller
    {
        private readonly SqliteContext _context;
        private readonly ILogger<AppointmentsController> _logger;
        private readonly ICaptchaValidator _captchaValidator;
        //сервіс по відправці електронної пошти
        private readonly SendEmailService _sendEmailService;

        public AdminController(
            ILogger<AppointmentsController> logger,
            SqliteContext context,
            ICaptchaValidator captchaValidator,
            SendEmailService sendEmailService)
        {
            _context = context;
            _logger = logger;
            _captchaValidator = captchaValidator;
            _sendEmailService = sendEmailService;
        }

        [HttpGet]
        public async Task<IActionResult> Pacients()
        {
            await _context.SaveChangesAsync();
            return View(await _context.Pacients.ToArrayAsync());
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id!=null)
            {
                var pacient = await _context.Pacients
                .Include(x => x.Complaints)
                .Include(z => z.PacientDiagnoses).ThenInclude(q => q.Diagnosis)
                //.Include(w => w.Objective)
                .FirstOrDefaultAsync(m => m.Id == id);

                _context.Pacients.Remove(pacient);
                _context.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Appointments/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var pacient = await _context.Pacients
                .Include(x => x.Complaints)
                .Include(z => z.PacientDiagnoses).ThenInclude(q => q.Diagnosis)
                .Include(w => w.Objective)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pacient == null)
            {
                return NotFound();
            }

            return View(pacient);
        }

        //редагуємо дані пацієнта
        public async Task<IActionResult> Edit(int id)
        {
            var pacient = await _context.Pacients
                .Include(x => x.Complaints)
                .Include(z => z.PacientDiagnoses).ThenInclude(q => q.Diagnosis)
                //.Include(w => w.Objective)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pacient == null)
            {
                return NotFound();
            }
            return View(pacient);
        }

        public IActionResult AddDiagnosis(int? id)
        {
            ViewBag.idPacient = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddDiagnosis(int id, DiagnosisDTO diagnosisDTO)
        {
            if (diagnosisDTO != null && id != 0)
            {
                var diag = new Diagnosis
                {
                    Description = diagnosisDTO.Description,
                    Name = diagnosisDTO.Name
                };
                var pacient = await _context.Pacients
                .Include(x => x.Complaints)
                .Include(z => z.PacientDiagnoses).ThenInclude(q => q.Diagnosis)
                //.Include(w => w.Objective)
                .FirstOrDefaultAsync(m => m.Id == id);
                _context.Diagnoses.Add(diag);
                await _context.SaveChangesAsync();
                _context.PacientDiagnoses.Add(
                    new PacientDiagnosis
                    {
                        DiagnosisId = diag.Id,
                        PacientId = id,
                    });
                await _context.SaveChangesAsync();
            }
            ViewBag.PacientId = id;
            return RedirectToAction("Edit", new { id });
        }

        [HttpGet]
        public IActionResult DelDiagnosis(int? Id)
        {
            var res = _context.PacientDiagnoses.FirstOrDefault(x => x.id == Id);
            _context.Diagnoses.Remove(
                _context.Diagnoses.FirstOrDefault(e => e.Id == res.DiagnosisId));
            _context.SaveChanges();
            return RedirectToAction("Edit", new { id =  res.PacientId});
        }

        [HttpPost]
        public async Task<IActionResult> SaveChanges()
        {
            await _context.SaveChangesAsync();
            return View("Pacients");
        }

        public IActionResult SaveModifiedData(int? Id)
        {
            _context.SaveChangesAsync();
            return RedirectToAction("Edit", new { id = Id });
        }
    }
}
