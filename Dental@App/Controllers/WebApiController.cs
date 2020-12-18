using System.Linq;
using Dental_App.DataContext;
using Dental_App.Models;
using Dental_App.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dental_App.Controllers
{
    public class WebApiController : Controller
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public WebApiController(IAppointmentRepository appointmentRepository,
            IEmployeeRepository employeeRepository, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _appointmentRepository = appointmentRepository;
            _employeeRepository = employeeRepository;
            _userManager = userManager;
            _context = context;
        }

        public JsonResult GetAllAppointmentJson()
        {
            var res = _appointmentRepository.GetAllAppointments()
                .OrderBy(p => p.AppointmentID)
                .Where(k => k.UserName == "jwayne@gmail.com" && k.EmployeeEmail.Equals("dpodsiadlo@klinika.pl"));
            return Json(res);
        }

        public ObjectResult GetAppointment()
        {
            Appointment app1 = _appointmentRepository.GetAppointment(81);

            var app2 = _appointmentRepository.GetAppointment(81);

            var app3 = _context.Appointments.FirstOrDefault(p => p.Employee.FirstName.Equals("Karolina"));

            return new ObjectResult(app3);
        }

        public JsonResult GetEmployee()
        {
            var r = _employeeRepository.GetEmployee(7);
            return Json(r);
        }

        public JsonResult GetUsers()
        {
            var listUsers = _userManager.Users;
            return Json(listUsers);
        }

    }
}