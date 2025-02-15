using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vacation_System.Entity
{
    public class RequestState
    {
        [Key]
        public int StateId { get; set; }

        [Required]
        [MaxLength(10)]
        public string Name { get; set; } = null!;
    }
}
