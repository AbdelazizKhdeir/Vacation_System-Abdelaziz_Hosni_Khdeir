using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vacation_System.Data;
using Vacation_System.Entity;

namespace Vacation_System.Services
{
    public class VacationRequestService
    {
        private readonly AppDbContext _context;

        public VacationRequestService(AppDbContext context) => _context = context;

        public bool SubmitRequest(VacationRequest request)
        {
            // Check for overlapping requests
            var overlaps = _context.VacationRequests
                .Any(vr => vr.EmployeeNumber == request.EmployeeNumber &&
                         (vr.StartDate <= request.EndDate && vr.EndDate >= request.StartDate));

            if (!overlaps)
            {
                _context.VacationRequests.Add(request);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public void ApproveRequest(int requestId, string approverNumber)
        {
            var request = _context.VacationRequests
                .Include(vr => vr.Employee)
                .FirstOrDefault(vr => vr.RequestId == requestId);

            if (request?.Employee.ReportedToEmployeeNumber == approverNumber)
            {
                request.RequestStateId = 2; // Approved
                request.ApprovedByEmployeeNumber = approverNumber;
                _context.SaveChanges();
            }
        }

        internal void DeclinedByEmployeeNumber(int requestId, string? reportedToEmployeeNumber)
        {
            throw new NotImplementedException();
        }
    }
}
