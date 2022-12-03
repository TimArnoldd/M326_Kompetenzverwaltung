using BL;
using Kompetenzverwaltung.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Kompetenzverwaltung.Controllers
{
    public class HomeController : Controller
    {
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
            cvm.UserCompetences = B.GetUsersUserCompetences(userId);
            cvm.CompetenceAreas = B.GetAllAreasWithCompetencesLoaded();
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
            B.InvertUserCompetencePinState(userId, id);
            return RedirectToAction("Index");
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