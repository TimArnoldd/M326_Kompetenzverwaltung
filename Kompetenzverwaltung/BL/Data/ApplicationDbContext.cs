using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BL.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString: "Server=.\\SQLEXPRESS;Database=M326_Kompetenzverwaltung;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}
