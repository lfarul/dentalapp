using System;
using System.ComponentModel.DataAnnotations;

namespace DentalApp.Models
{
    public class Appointment
    {
        public int AppointmentID { get; set; }
        public string UserName { get; set; }

        //[DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Appointment Start")]
        public DateTime? AppointmentStart { get; set; }

        //[DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Appointment Finish")]
        public DateTime? AppointmentFinish { get; set; }

        // navigation properties - wizyta może dotyczyć tylko jednego lekarza i pacjenta (lekarz przyjmuje jednego pacjenta)
        // podcza wizyty przyjmuje jeden lekarz
        public int EmployeeID { get; set; }
        public Employee Employee { get; set; }
        public string EmployeeEmail { get; set; }

    }
}
