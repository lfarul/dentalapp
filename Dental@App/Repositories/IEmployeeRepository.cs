using Dental_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dental_App.Repositories
{
    public interface IEmployeeRepository
    {
        Employee GetEmployee(int EmployeeID);
        IEnumerable<Employee> GetAllEmployees();
        Employee Add(Employee employee);
        Employee Update(Employee employeeUpdate);
        Employee Delete(int Id);
    }
}
