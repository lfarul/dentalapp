using Dental_App.Models;
using System.Collections.Generic;


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
