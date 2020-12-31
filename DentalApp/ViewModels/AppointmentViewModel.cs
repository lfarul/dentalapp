using DentalApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DentalApp.ViewModels
{
    public class AppointmentViewModel
    {
        public AppointmentViewModel()

        {
            Users = new List<string>();
        }

        public int AppointmentID { get; set; }
        public Employee Employee { get; set; }
        public int EmployeeID { get; set; }
        public string Specialty { get; set; }
        public string UserName { get; set; }
        public string UserID { get; set; }

        public int AppointmentCount { get; set; }

        [Required(ErrorMessage = "Please provide date of appointment")]
        [Display(Name = "Appointment Start")]
        public DateTime? AppointmentStart { get; set; }

        public DateTime? AppointmentFinish { get; set; }

        public List<string> Users { get; set; }

        public bool IsSelected { get; set; }

        public string EmployeeEmail { get; set; }
    }
}
