using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vacation_System.Entity
{
    public class Position
    {
        [Key]
        public int PositionId { get; set; }

        [Required, MaxLength(30)]
        public string PositionName { get; set; } = string.Empty;

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }

}
