using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    public class Profession
    {
        [Required]
        public int Id { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
    }
}
