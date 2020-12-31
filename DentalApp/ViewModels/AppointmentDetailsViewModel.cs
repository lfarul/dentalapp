using DentalApp.Models;
using System;

namespace DentalApp.ViewModels
{
    public class AppointmentDetailsViewModel
    {
        public Appointment Appointment { get; set; }
        public int AppointmentID { get; set; }
        public Employee Employee { get; set; }
        public int EmployeeID { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public DateTime? AppointmentStart { get; set; }
        public DateTime? AppointmentFinish { get; set; }
    }
}
