using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vacation_System.Data;

namespace Vacation_System.Services
{
    public class ReportService
    {
        private readonly AppDbContext _context;

        public ReportService(AppDbContext context) => _context = context;

        public IEnumerable<object> GetPendingRequestsForManager(string managerNumber)
        {
            return _context.VacationRequests
                .Where(vr => vr.Employee.ReportedToEmployeeNumber == managerNumber &&
                           vr.RequestStateId == 1)
                .Select(vr => new
                {
                    vr.Description,
                    vr.EmployeeNumber,
                    vr.Employee.EmployeeName,
                    vr.RequestSubmissionDate,
                    Duration = $"{vr.TotalVacationDays} days",
                    vr.StartDate,
                    vr.EndDate,
                    vr.Employee.Salary
                })
                .ToList();
        }
    }
}
