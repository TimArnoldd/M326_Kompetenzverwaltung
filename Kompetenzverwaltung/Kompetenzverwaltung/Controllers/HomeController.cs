using BL.Data;
using BL.Models;
using Kompetenzverwaltung.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace Kompetenzverwaltung.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context = new();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index()
        {
            CompetenceViewModel cvm = new();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            cvm.UserCompetences = _context.UserCompetences.Where(x => x.User.Id == userId).ToList();
            cvm.CompetenceAreas = _context.CompetenceAreas.Include(x => x.Competences).ToList();
            return View(cvm);
        }

        /// <summary>
        /// Requires the Id of the Competence (not the UserCompetence)
        /// </summary>
        [Authorize]
        public IActionResult Details(int id)
        {
            return View();
        }

        [Authorize]
        public IActionResult InvertPinState(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Competence? competence = _context.Competences.Find(id);
            if (competence == null)
                return RedirectToAction("Index");

            UserCompetence? userCompetence = _context.UserCompetences.FirstOrDefault(x => x.CompetenceId == id && x.User.Id == userId);
            if (userCompetence == null)
            {
                userCompetence = new()
                {
                    Competence = competence,
                    User = _context.Users.First(x => x.Id == userId)
                };
                _context.UserCompetences.Add(userCompetence);
            }
            userCompetence.Pinned = !userCompetence.Pinned;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult DetailOfCompetence()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}