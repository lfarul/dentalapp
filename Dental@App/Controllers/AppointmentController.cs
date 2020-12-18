using System;
using System.Linq;
using System.Threading.Tasks;
using Dental_App.DataContext;
using Dental_App.Models;
using Dental_App.Repositories;
using Dental_App.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;

namespace Dental_App.Controllers
{
    public class AppointmentController : Controller
    {
        // atrybut readonly uniemożliwia przypadkowe nadanie nowej wartości polu
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        public AppointmentController(IAppointmentRepository appointmentRepository, IEmployeeRepository employeeRepository,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
        {
            _appointmentRepository = appointmentRepository;
            _employeeRepository = employeeRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        // Tutaj wyświetlam wszystkie wizyty
        //[Authorize(Roles ="Admin")]
        public ViewResult GetAllAppointments()
        {
            var model = _appointmentRepository.GetAllAppointments()
                .OrderByDescending(p => p.AppointmentStart);
            ViewBag.TerazJest = DateTime.Now;
            return View(model);
        }

        //Wizytę umawia Pacjent - get
        [HttpGet]
        [Authorize]
        public ViewResult CreateAppointment(int id)
        {
            AppointmentViewModel appointment = new AppointmentViewModel();
            appointment.EmployeeID = _employeeRepository.GetEmployee(id).EmployeeID;
            appointment.EmployeeEmail = _employeeRepository.GetEmployee(id).Email;
            //parsowanie do int
            appointment.UserID = _userManager.GetUserId(User);
            return View(appointment);
        }


        // Wizytę umawia Pacjent - post - kiedy wybieram datę i godzinę spotkania i naciskam - Umawiam - wywołuję metodę post
        [HttpPost]
        [Authorize]
        public RedirectToActionResult CreateAppointment(AppointmentViewModel appointment)
        {
            if (appointment.AppointmentStart < DateTime.Now)
            {
                return RedirectToAction("AppointmentLate");
            }

            else if (_context.Appointments.Where(x =>
            x.AppointmentStart == appointment.AppointmentStart &&
            x.Employee.EmployeeID == appointment.Employee.EmployeeID).Any())
            {
                return RedirectToAction("AppointmentExists");
            }

            else
            {
                Models.Appointment appointmentModel = new Models.Appointment();

                appointmentModel.Employee = _context.Employees.FirstOrDefault(x => x.EmployeeID == appointment.Employee.EmployeeID);
                appointmentModel.UserName = _userManager.GetUserName(User);
                appointmentModel.EmployeeEmail = appointmentModel.Employee.Email;
                appointmentModel.AppointmentStart = appointment.AppointmentStart;
                appointmentModel.AppointmentFinish = appointment.AppointmentFinish;
       

                if (appointmentModel.AppointmentStart.HasValue)
                {

                    appointment.AppointmentFinish = appointmentModel.AppointmentStart.Value.AddMinutes(15);
                    appointmentModel.AppointmentFinish = appointment.AppointmentFinish;
                    
                    if (_context.Appointments.Where(x =>
                    x.AppointmentStart <= appointment.AppointmentStart &&
                    x.AppointmentFinish >= appointment.AppointmentStart &&
                    x.Employee.EmployeeID == appointment.Employee.EmployeeID).Any())
                    {
                        return RedirectToAction("AppointmentExists");
                    }

                    else if (_context.Appointments.Where(x =>
                     x.AppointmentStart <= appointment.AppointmentFinish &&
                     x.AppointmentFinish >= appointment.AppointmentFinish &&
                     x.Employee.EmployeeID == appointment.Employee.EmployeeID).Any())
                    {
                        return RedirectToAction("AppointmentExists");
                    }


                    Appointment newAppointment = _appointmentRepository.Add(appointmentModel);
                    _context.SaveChanges();

                }

            }
            return RedirectToAction("MyAppointment");
        }


        // Tutaj Pacjent umawia wizytę - jak wybiore lekarza i naciskam - Umów wiytę to wywołuję tę metodę
        public IActionResult Appointment(int id)
        {
            AppointmentViewModel appointmentViewModel = new AppointmentViewModel()
            {
                UserID = _userManager.GetUserId(User),
                Employee = _employeeRepository.GetEmployee(id),
                EmployeeID = _employeeRepository.GetEmployee (id).EmployeeID,
                EmployeeEmail = _employeeRepository.GetEmployee(id).Email,
            };

            return View(appointmentViewModel);
        }


        // Po umówieniu wizyty, Pacjent przechodzi do Podsumowania wizyty - AppointmentDetails
        public ViewResult AppointmentDetails(int id)
        {
            AppointmentDetailsViewModel appointmentDetailsViewModel = new AppointmentDetailsViewModel()
            {
                UserID = _userManager.GetUserId(User),
                Appointment = _appointmentRepository.GetAppointment(id),
                Employee = _employeeRepository.GetEmployee(id),
            };

            return View(appointmentDetailsViewModel);
        }

        // Po umówieniu wizyty, podsumowanie można zapisać do PDF
        public async Task<IActionResult> AppointmentPDF(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(m => m.AppointmentID == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return new ViewAsPdf(appointment);
        }

        // Tutaj wyświetlają się wizyty tylko dla zalogowanego pacjenta
        public async Task<IActionResult> MyAppointment(int id)
        {
            {
                ApplicationUser user = await GetCurrentUserAsync();
                var appointment = _appointmentRepository.GetAllAppointments()
                    .Where(p => p.UserName == user.UserName)
                    .OrderByDescending(p => p.AppointmentStart);

                ViewBag.TerazJest = DateTime.Now;
                return View(appointment);
            }

        }


        // Tutaj wyświetlają się wizyty tylko dla zalogowanego lekarza
        public async Task<IActionResult> DoctorAppointment(int id)
        {
            ApplicationUser user = await GetCurrentUserAsync();
            var appointment = _appointmentRepository.GetAllAppointments()
                .Where(p => p.EmployeeEmail == user.UserName)
                .OrderByDescending(p => p.AppointmentStart);

            ViewBag.TerazJest = DateTime.Now;
            return View(appointment);
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // Edytuję wizytę
        [HttpGet]
        public ViewResult EditAppointment(int id)
        {
            Appointment appointment = _appointmentRepository.GetAppointment(id);
            AppointmentEditViewModel appointmentEditViewModel = new AppointmentEditViewModel
            {
                AppointmentID = appointment.AppointmentID,
                EmployeeID = appointment.EmployeeID,
                UserName = appointment.UserName,

                AppointmentStart = appointment.AppointmentStart,
                AppointmentFinish = appointment.AppointmentFinish,
            };
            return View(appointmentEditViewModel);
        }

        // Dokonuję zmian w edytowanej wizycie
        [HttpPost]
        public IActionResult EditAppointment(AppointmentEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Appointment appointment = _appointmentRepository.GetAppointment(model.AppointmentID);
                appointment.EmployeeID = model.EmployeeID;
                appointment.UserName = model.UserName;
                appointment.AppointmentStart = model.AppointmentStart;

                appointment.AppointmentFinish = model.AppointmentStart.Value.AddMinutes(15);
                appointment.AppointmentFinish = appointment.AppointmentFinish;
                model.AppointmentFinish = appointment.AppointmentFinish;


                if (appointment.AppointmentStart < DateTime.Now)
                {
                    return RedirectToAction("AppointmentLate");
                }

                else if (_context.Appointments.Where(x =>
                x.AppointmentStart <= appointment.AppointmentStart &&
                x.AppointmentFinish >= appointment.AppointmentStart).Any())
                {
                    return RedirectToAction("AppointmentExists");
                }

                else if (_context.Appointments.Where(x =>
                x.AppointmentStart <= appointment.AppointmentFinish &&
                x.AppointmentFinish >= appointment.AppointmentFinish).Any())
                {
                    return RedirectToAction("AppointmentExists");
                }

                _appointmentRepository.Update(appointment);

                if (_signInManager.IsSignedIn(User) && !(User.IsInRole("Admin")) && !(User.IsInRole("Lekarz")))
                {
                    return RedirectToAction("MyAppointment");
                }

                else if (_signInManager.IsSignedIn(User) && User.IsInRole("Lekarz"))
                {
                    return RedirectToAction("DoctorAppointment");
                }

                else if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                {
                    return RedirectToAction("GetAllAppointments");
                }
            }
            return View();
        }

        // Pokazuje szczegóły dla wizyty
        public async Task<IActionResult> DetailAppointment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(m => m.AppointmentID == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // Usuwa wizytę
        public ActionResult DeleteAppointment(int id)
        {
            // before deleting a Pacjent, we need to find them first
            Appointment appointment = _context.Appointments.Find(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                _context.SaveChanges();
            }
            return RedirectToAction("MyAppointment");
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.AppointmentID == id);
        }

        public IActionResult AppointmentExists()
        {
            return View();
        }

        public IActionResult AppointmentLate()
        {
            return View();
        }


        public async Task<IActionResult> ReportDoctor(int id)
        {
            ApplicationUser user = await GetCurrentUserAsync();
            var appointment = _appointmentRepository.GetAllAppointments()
                .Where(p => p.EmployeeEmail == user.UserName)
                .OrderByDescending(p => p.AppointmentStart);

            ViewBag.TerazJest = DateTime.Now;
            return View(appointment);
        }

        public ViewResult ReportAdmin()
        {
            var model = _appointmentRepository.GetAllAppointments()
                .OrderByDescending(p => p.AppointmentStart);
            ViewBag.TerazJest = DateTime.Now;
            return View(model);
        }

        public async Task<IActionResult> ReportPatient(int id)
        {
            ApplicationUser user = await GetCurrentUserAsync();
            var appointment = _appointmentRepository.GetAllAppointments()
                .Where(p => p.UserName == user.UserName)
                .OrderByDescending(p => p.AppointmentStart);

            ViewBag.TerazJest = DateTime.Now;
            return View(appointment);
        }

    }
}
