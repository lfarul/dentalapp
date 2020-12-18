using Dental_App.DataContext;
using Dental_App.Models;
using System.Collections.Generic;

namespace Dental_App.Repositories
{
    public class EmployeeRepositoryImplementation : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;
        public EmployeeRepositoryImplementation(ApplicationDbContext context)
        {
            _context = context;
        }
        public Employee Add(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
            return employee;

        }

        public Employee Delete(int Id)
        {
            // before deleting a Doctor, we need to find them first
            Employee employee = _context.Employees.Find(Id);
           
            if(employee != null)
            {
                _context.Employees.Remove(employee);
                _context.SaveChanges();
            }
            return employee;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _context.Employees;
        }

        public Employee GetEmployee(int EmployeeID)
        {
            return _context.Employees.Find(EmployeeID);
        }

        public Employee Update(Employee employeeUpdate)
        {
            var employee = _context.Employees.Attach(employeeUpdate);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return employeeUpdate;
        }
    }
}
