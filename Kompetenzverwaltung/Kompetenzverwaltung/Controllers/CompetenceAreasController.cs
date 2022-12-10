using BL;
using BL.Models;
using Kompetenzverwaltung.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kompetenzverwaltung.Controllers
{
    // TODO: [Authorize(Roles = "Administrator")]
    [Authorize]
    public class CompetenceAreasController : Controller
    {
        public IActionResult Index()
        {
            var dbAreas = B.GetAllAreas();
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
            var dbArea = B.GetCompetenceArea(id);
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
                B.CreateCompetenceArea(area);
            }
            else
            {
                B.UpdateCompetenceArea(area);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            B.DeleteCompetenceArea(id);
            return RedirectToAction("Index");
        }
    }
}
