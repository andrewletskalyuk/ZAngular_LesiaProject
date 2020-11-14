using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClassLibraryDbContext;
using Models.Model;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;
using GoogleReCaptcha.V3.Interface;
using StartOfEnding.Services;

namespace StartOfEnding.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly SqliteContext _context;
        private readonly ILogger<AppointmentsController> _logger;
        private readonly ICaptchaValidator _captchaValidator;
        //сервіс по відправці електронної пошти
        private readonly SendEmailService _sendEmailService;

        public AppointmentsController(
            ILogger<AppointmentsController> logger, 
            SqliteContext context, 
            ICaptchaValidator captchaValidator,
            SendEmailService sendEmailService
            )
        {
            _context = context;
            _logger = logger;
            _captchaValidator = captchaValidator;
            _sendEmailService = sendEmailService;
        }

        // GET: Appointments
        //тут треба буде зробити тыльки для адміна
        public async Task<IActionResult> ListAppointments()
        {
            return View(await _context.Appointments.ToListAsync());
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointments/Create
        public IActionResult AddAppointment()
        {
            return View();
        }

        [HttpPost]
        [Obsolete]
        public async Task<IActionResult> AddAppointment(Appointment appointment, string captcha)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(captcha))
            {
                ModelState.AddModelError("captcha", "Captcha validation failed");
            }
            if (ModelState.IsValid)
            {
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                ViewBag.NameUser = appointment.Name;

                //відправка повідомлення на пошту, хто записався на прийом - використаємо сервіс
                string fromTitle = "Гоголь Л.В. лікар-перідатр";
                string fromEmail = "doctorgogol@gmail.com";
                string subject = "Запис на прийом";
                _sendEmailService.SendEmailCustom(appointment, fromTitle, fromEmail, subject);

                return View("Confirmation");
            }
            return View();
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Phone,Email,Message,DateWhenAdded")] Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(appointment);
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction("ListAppointments");
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }
    }
}
