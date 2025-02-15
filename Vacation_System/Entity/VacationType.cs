using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vacation_System.Entity
{
    public class VacationType
    {
        [Key]
        [MaxLength(1)]
        public string Code { get; set; } = null!;

        [MaxLength(20)]
        public string Name { get; set; } = null!;
    }
}
