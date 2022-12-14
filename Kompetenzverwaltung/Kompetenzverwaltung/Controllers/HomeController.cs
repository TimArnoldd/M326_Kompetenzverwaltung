using BL;
using BL.Enums;
using Kompetenzverwaltung.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
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
            UserOverviewViewModel cvm = new();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            cvm.UserCompetences = B.GetUsersUserCompetences(userId);
            cvm.CompetenceAreas = B.GetAllAreasWithCompetencesLoaded();
            return View(cvm);
        }


        [Authorize]
        public IActionResult Details(int id)
        {
            DetailViewModel cvm = new();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userCompetence = B.GetUserCompetence(userId, id);
            var user = B.GetUser(userId);
            if (userCompetence == null || user == null)
                return RedirectToAction("Index");
            var resources = B.GetResourcesFromCompetence(id);
            cvm.Resources = resources;
            cvm.UserCompetence = userCompetence;
            cvm.CompetenceState = userCompetence.State;
            cvm.UserCompetence.User = user;
            
            

            return View(cvm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(DetailViewModel cvm)
        {
            B.UpdateCompetenceState(cvm.UserCompetence);
            return RedirectToAction("Details");
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