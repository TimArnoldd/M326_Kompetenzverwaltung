using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual Profession? Profession { get; set; }
        public virtual ICollection<UserCompetence> UserCompetences { get; set; }
    }
}
