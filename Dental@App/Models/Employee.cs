using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dental_App.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }

        [Required(ErrorMessage = "Please provide first name")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please provide last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please provide specialty")]
        public string Specialty { get; set; }

        [Required(ErrorMessage = "Please provide email")]
        [RegularExpression(@"^[a-zA-Z0-9_+.-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "This is not an appropriate email format.")]
        public string Email { get; set; }

        public string Description { get; set; }

        [Display(Name = "Photo")]
        public string PhotoPath { get; set; }

        // navigation properties - lekarz może mieć kilka wizyt
        public ICollection<Appointment> Appointments { get; set; }
    }
}
