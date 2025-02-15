using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vacation_System.Data;
using Vacation_System.Entity;

namespace Vacation_System.Services
{
    public class EmployeeService
    {
            private readonly AppDbContext _context;

            public EmployeeService(AppDbContext context) => _context = context;

            public void UpdateEmployee(string empNumber, Action<Employee> updateAction)
            {
                var employee = _context.Employees.FirstOrDefault(e => e.EmployeeNumber == empNumber);
                if (employee != null)
                {
                    updateAction(employee);
                    _context.SaveChanges();
                }
            }

            public void UpdateVacationDays(string empNumber, int daysUsed)
            {
                var employee = _context.Employees.Find(empNumber);
                if (employee != null)
                {
                    employee.VacationDaysLeft -= daysUsed;
                    _context.SaveChanges();
                }
            }
        
    }
}
