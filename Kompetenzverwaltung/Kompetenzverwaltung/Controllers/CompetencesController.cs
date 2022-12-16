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
        private readonly B b;
        public CompetencesController()
        {
            b = new();
        }

        [HttpGet]
        public IActionResult Index(int id)
        {
            var competences = b.GetAllCompetencesFromArea(id);
            var area = b.GetCompetenceArea(id);
            if (area == null)
                return RedirectToAction("Index", "Home");

            CompetencesViewModel vm = new()
            {
                Competences = competences.ToList(),
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
            var dbCompetence = b.GetCompetence(id);
            var dbCompetenceArea = b.GetCompetenceAreaFromCompetenceId(id);
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
        public IActionResult Edit(CompetenceViewModel vm)
        {
            if (!ModelState.IsValid)
                return View("Competence", vm);

            var competenceArea = b.GetCompetenceArea(vm.CompetenceAreaId);
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
                b.CreateCompetence(competence);
            }
            else
            {
                b.UpdateCompetence(competence);
            }

            return RedirectToAction("Index", new { id = competence.CompetenceArea.Id });
        }

        public IActionResult Delete(int id)
        {
            var competenceArea = b.GetCompetenceAreaFromCompetenceId(id);
            b.DeleteCompetence(id);
            return RedirectToAction("Index", new { id = competenceArea?.Id });
        }
    }
}
