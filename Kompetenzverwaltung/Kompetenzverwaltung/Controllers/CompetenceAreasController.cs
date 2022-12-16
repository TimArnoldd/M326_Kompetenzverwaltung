using BL;
using BL.Models;
using Kompetenzverwaltung.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kompetenzverwaltung.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CompetenceAreasController : Controller
    {
        private readonly B b;
        public CompetenceAreasController()
        {
            b = new();
        }

        public IActionResult Index()
        {
            var dbAreas = b.GetAllAreas();
            var areas = dbAreas.Select(x => new CompetenceAreaViewModel { Id = x.Id, Name = x.Name }).ToList();
            return View(areas);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("CompetenceArea", new CompetenceAreaViewModel());
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var dbArea = b.GetCompetenceArea(id);
            if (dbArea == null)
                return RedirectToAction("Index");

            CompetenceAreaViewModel area = new() { Id = dbArea.Id, Name = dbArea.Name };
            return View("CompetenceArea", area);
        }

        [HttpPost]
        public IActionResult Edit(CompetenceAreaViewModel areaViewModel)
        {
            if (!ModelState.IsValid)
                return View("CompetenceArea", areaViewModel);

            CompetenceArea area = new() { Id = areaViewModel.Id, Name = areaViewModel.Name };

            if (area.Id == 0) // New entity
            {
                b.CreateCompetenceArea(area);
            }
            else
            {
                b.UpdateCompetenceArea(area);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            b.DeleteCompetenceArea(id);
            return RedirectToAction("Index");
        }
    }
}
