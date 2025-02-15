using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vacation_System.Entity
{
    public class Employee
    {

        [Key, MaxLength(6)]
        public string EmployeeNumber { get; set; } = null!;

        [Required, MaxLength(20)]
        public string EmployeeName { get; set; } = null!;

        [Required]
        public int DepartmentId { get; set; }
        public Department Department { get; set; } = null!;

        [Required]
        public int PositionId { get; set; }
        public Position Position { get; set; } = null!;

        [Required, MaxLength(1)]
        public string GenderCode { get; set; } = "M";

        public string? ReportedToEmployeeNumber { get; set; }

        [Range(0, 24)]
        public int VacationDaysLeft { get; set; } = 24;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Salary { get; set; }

        public Employee() { }

        public Employee(string number, string name)
        {
            EmployeeNumber = number;
            EmployeeName = name;
        }

        public ICollection<VacationRequest> VacationRequests { get; set; } = new List<VacationRequest>();
    }

}
