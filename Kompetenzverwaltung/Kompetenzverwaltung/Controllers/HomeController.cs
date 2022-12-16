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
        private readonly B b;

        public HomeController()
        {
            b = new B();
        }

        [Authorize]
        public IActionResult Index()
        {
            UserOverviewViewModel cvm = new();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            cvm.UserCompetences = b.GetUsersUserCompetences(userId).ToList();
            cvm.CompetenceAreas = b.GetAllAreasWithCompetencesLoaded().ToList();
            return View(cvm);
        }


        [Authorize]
        public IActionResult Details(int id)
        {
            DetailViewModel cvm = new();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userCompetence = b.GetUserCompetence(userId, id);
            var user = b.GetUser(userId);
            if (userCompetence == null || user == null)
                return RedirectToAction("Index");
            var resources = b.GetResourcesFromCompetence(id);
            cvm.Resources = resources.ToList();
            cvm.UserCompetence = userCompetence;
            cvm.CompetenceState = userCompetence.State;
            cvm.UserCompetence.User = user;
            
            

            return View(cvm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(DetailViewModel cvm)
        {
            b.UpdateCompetenceState(cvm.UserCompetence);
            return RedirectToAction("Details");
        }

        [Authorize]
        public IActionResult InvertPinState(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            b.InvertUserCompetencePinState(userId, id);
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