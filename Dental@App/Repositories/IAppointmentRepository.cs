using Dental_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dental_App.Repositories
{
    public interface IAppointmentRepository
    {
        Appointment GetAppointment(int AppointmentID);
        IEnumerable<Appointment> GetAllAppointments();
        Appointment Add(Appointment appointment);
        Appointment Update(Appointment appointmentUpdate);
        Appointment Delete(int Id);
    }
}
