using BL;
using BL.Models;
using Kompetenzverwaltung.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kompetenzverwaltung.Controllers
{
    [Authorize]
    public class CompetencesController : Controller
    {
        [HttpGet]
        public IActionResult Index(int id)
        {
            var competences = B.GetAllCompetencesFromArea(id);
            var area = B.GetCompetenceArea(id);
            if (area == null)
                return RedirectToAction("Index", "Home");

            CompetencesViewModel vm = new()
            {
                Competences = competences,
                CompetenceAreaName = area.Name,
                CompetenceAreaId = area.Id
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult Create(int id)
        {
            return View("Competence", new CompetenceViewModel() { CompetenceAreaId = id });
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var dbCompetence = B.GetCompetence(id);
            var dbCompetenceArea = B.GetCompetenceAreaFromCompetenceId(id);
            if (dbCompetence == null ||
                dbCompetenceArea == null)
                return RedirectToAction("Index", "Home");

            CompetenceViewModel vm = new()
            {
                CompetenceId = dbCompetence.Id,
                CompetenceAreaId = dbCompetenceArea.Id,
                Name = dbCompetence.Name,
                Description = dbCompetence.Description,
                Level = dbCompetence.Level
            };

            return View("Competence", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CompetenceViewModel vm)
        {
            if (!ModelState.IsValid)
                return View("Competence", vm);

            var competenceArea = B.GetCompetenceArea(vm.CompetenceAreaId);
            if (competenceArea == null)
                return RedirectToAction("Index", "Home");

            Competence competence = new()
            {
                Id = vm.CompetenceId,
                Name = vm.Name,
                Description = vm.Description,
                Level = vm.Level,
                CompetenceArea = competenceArea
            };

            if (competence.Id == 0) // New entity
            {
                B.CreateCompetence(competence);
            }
            else
            {
                B.UpdateCompetence(competence);
            }

            return RedirectToAction("Index", new { id = competence.CompetenceArea.Id });
        }

        public IActionResult Delete(int id)
        {
            var competenceArea = B.GetCompetenceAreaFromCompetenceId(id);
            B.DeleteCompetence(id);
            return RedirectToAction("Index", new { id = competenceArea?.Id });
        }
    }
}
