using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Kompetenzverwaltung.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [DisplayName("Administrator")]
        public bool IsAdministrator { get; set; }

    }
}
