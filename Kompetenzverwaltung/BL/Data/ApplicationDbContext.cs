﻿using BL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BL.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Competence> Competences { get; set; }
        public DbSet<CompetenceArea> CompetenceAreas { get; set; }
        public DbSet<UserCompetence> UserCompetences { get; set; }
        public DbSet<Profession> Professions { get; set; }
        public DbSet<Resource> Resources { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString: "Server=.\\SQLEXPRESS;Database=M326_Kompetenzverwaltung;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}
