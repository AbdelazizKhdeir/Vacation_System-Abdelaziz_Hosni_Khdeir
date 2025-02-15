using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vacation_System.Entity
{
    public class VacationRequest
    {
        [Key]
        public int RequestId { get; set; }

        [Required]
        public DateTime RequestSubmissionDate { get; set; }

        [Required, MaxLength(100)]
        public string Description { get; set; } = null!;

        [Required, MaxLength(6)]
        public string EmployeeNumber { get; set; } = null!;
        public Employee Employee { get; set; } = null!;

        [Required, MaxLength(1)]
        public string VacationTypeCode { get; set; } = null!;
        public VacationType VacationType { get; set; } = null!;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public int TotalVacationDays { get; set; }

        [Required]
        public int RequestStateId { get; set; }
        public RequestState RequestState { get; set; } = null!;

        public string? ApprovedByEmployeeNumber { get; set; }
        public string? DeclinedByEmployeeNumber { get; set; }
    }

}
