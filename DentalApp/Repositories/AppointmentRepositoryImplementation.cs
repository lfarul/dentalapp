using DentalApp.DataContext;
using DentalApp.Models;
using System.Collections.Generic;


namespace DentalApp.Repositories
{
    public class AppointmentRepositoryImplementation : IAppointmentRepository
    {

        private readonly ApplicationDbContext _context;
        public AppointmentRepositoryImplementation(ApplicationDbContext context)
        {
            _context = context;
        }


        public Appointment Add(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            _context.SaveChanges();
            return (appointment);

        }

        public Appointment Delete(int Id)
        {
            // before deleting a Doctor, we need to find them first
            Appointment appointment = _context.Appointments.Find(Id);

            if(appointment != null)
            {
                _context.Appointments.Remove(appointment);
                _context.SaveChanges();
            }
            return (appointment);
        }

        public IEnumerable<Appointment> GetAllAppointments()
        {
            return _context.Appointments;
        }

        public Appointment GetAppointment(int AppointmentID)
        {
            return _context.Appointments.Find(AppointmentID);
        }

        public Appointment Update(Appointment appointmentUpdate)
        {
            var appointment = _context.Appointments.Attach(appointmentUpdate);
            appointment.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return appointmentUpdate;
        }
    }
}
