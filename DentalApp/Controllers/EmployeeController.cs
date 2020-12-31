using System;
using System.IO;
using System.Linq;
using DentalApp.DataContext;
using DentalApp.Models;
using DentalApp.Repositories;
using DentalApp.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DentalApp.Controllers
{
    public class EmployeeController : Controller
    {
    // dependency injection przez konstruktor
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public EmployeeController(IEmployeeRepository employeeRepository, IHostingEnvironment hostingEnvironment,
            UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _employeeRepository = employeeRepository;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
            _context = context;
        }

        // Lista lekarzy dla pacjentów
        public ViewResult NewList()
        {
            var model = _employeeRepository.GetAllEmployees().Where(p => p.Specialty != "Asystentka");
            return View(model);
        }

        // Lista prcowników dla admina
        public ViewResult List()
        {
            var model = _employeeRepository.GetAllEmployees();
            return View(model);
        }

        public ViewResult Details(int id)
        {
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = _employeeRepository.GetEmployee(id),
                PageTitle = "Employee Details"
            };

            return View(homeDetailsViewModel);
        }


        [HttpGet]
        public ViewResult Edit(int id)
        {
            Employee employee = _employeeRepository.GetEmployee(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                EmployeeID = employee.EmployeeID,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Specialty = employee.Specialty,
                Email = employee.Email,
                Description = employee.Description,
                ExistingPhotoPath = employee.PhotoPath
            };
            return View(employeeEditViewModel);
        }


        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = _employeeRepository.GetEmployee(model.EmployeeID);
                employee.FirstName = model.FirstName;
                employee.LastName = model.LastName;
                employee.Specialty = model.Specialty;
                employee.Email = model.Email;
                employee.Description = model.Description;


                if (model.Photo != null)
                {
                    if (model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "Images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    employee.PhotoPath = ProcessUploadedFile(model);
                }

                _employeeRepository.Update(employee);

                return RedirectToAction("NewList");
            }
            return View();
        }

        private string ProcessUploadedFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;

            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model);
                Employee newEmployee = new Employee
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Specialty = model.Specialty,
                    Email = model.Email,
                    Description = model.Description,
                    PhotoPath = uniqueFileName
                };

                _employeeRepository.Add(newEmployee);

                return RedirectToAction("details", new { id = newEmployee.EmployeeID });
            }
            return View();
        }

        public Employee Delete(int id)
        {
            Employee employee = _context.Employees.Find(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                _context.SaveChanges();
            }
            return employee;
        }
    }
}
