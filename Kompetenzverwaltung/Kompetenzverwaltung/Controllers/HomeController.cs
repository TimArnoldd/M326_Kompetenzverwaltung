using BL.Data;
using BL.Models;
using Kompetenzverwaltung.Models;
using MessagePack.Formatters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Kompetenzverwaltung.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [Authorize]
        public IActionResult Index()
        {
            CompetenceViewModel model = new CompetenceViewModel();

            //Get current logged in user
            ApplicationUser user = new ApplicationUser();
            user.UserName = User.Identity.Name;
            if (user.UserName is not null)
            {
                //Create new user and assign where username is the same as logged in user
                ApplicationUser userfromDb = new ApplicationUser();
                userfromDb = db.Users.FirstOrDefault(u => u.UserName == user.UserName);
                model.UserId = userfromDb.Id;

                //Get CompetenceId from db.UserCompetences
                //Gets a List of all Usercompetences with same UserId
                List <UserCompetence> userCompetences = db.UserCompetences.Where(c => c.User.Id == model.UserId).ToList(); //why is userCompetence.Competence null?
                //List of all competences in DB
                List<Competence> allCompetencesFromDB = db.Competences.OrderBy(c => c.Level).ToList();
                model.Competences = new List<Competence>();
                //Pinned competences:
                List<UserCompetence> pinnedUserCompetences = db.UserCompetences.Where(c => c.User.Id == model.UserId && c.Pinned == true).ToList();
                model.PinnedCompetences = new List<Competence>();
                //Get all CompetenceAreas from db
                List<CompetenceArea> allCompetenceAreas = db.CompetenceAreas.ToList();
                model.CompatenceAreas = new List<CompetenceArea>();

                foreach (var competence in allCompetencesFromDB)
                {
                    if (userCompetences.Any(c => c.CompetenceId == competence.Id))
                    {
                        model.Competences.Add(competence);
                        //Add CompetenceArea of that Competence
                        if (model.CompatenceAreas.Any(c => c.Id != competence.CompetenceArea.Id) || model.CompatenceAreas.Count == 0)
                        {
                            model.CompatenceAreas.Add(competence.CompetenceArea);
                        }
                    }
                    if (pinnedUserCompetences.Any(c => c.CompetenceId == competence.Id))
                    {
                        model.PinnedCompetences.Add(competence);
                    }
                }
                
            }
            return View(model);
        }
        public IActionResult LabelCompetence(int id)
        {
            if (ModelState.IsValid)
            {
                Competence competence = db.Competences.Find(id);
                UserCompetence userCompetence = db.UserCompetences.FirstOrDefault(u => u.CompetenceId == id);
                if (userCompetence.Pinned == false)
                {
                    userCompetence.Pinned = true;
                }
                else if (userCompetence.Pinned == true)
                {
                    userCompetence.Pinned = false;
                }
                db.UserCompetences.Update(userCompetence);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public IActionResult DetailOfCompetence()
        {
            return View();
        }

        //For the _PartialCompetence partial view
        //public IActionResult CompetencePartial()
        //{
        //    CompetenceViewModel viewModel = new CompetenceViewModel();
        //    return PartialView("_PartialCompetence", CompetenceViewModel)
        //}

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