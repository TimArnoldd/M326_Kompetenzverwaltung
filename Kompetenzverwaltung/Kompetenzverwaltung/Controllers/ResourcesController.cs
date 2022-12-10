using BL;
using BL.Models;
using Kompetenzverwaltung.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kompetenzverwaltung.Controllers
{
    public class ResourcesController : Controller
    {
        [HttpGet]
        public IActionResult Index(int id)
        {
            if (id == 0)
                return RedirectToAction("Index", "Home");
            var resources = B.GetResourcesFromCompetence(id);
            ResourcesViewModel vm = new()
            {
                CompetenceId = id,
                Resources = resources.Select(x => new ResourceViewModel { ResourceId = x.Id, DisplayText = x.DisplayText, Link = x.Link }).ToList()
            };
            return View(vm);
        }

        [HttpGet]
        public IActionResult Create(int id)
        {
            return View("Resource", new ResourceViewModel() { CompetenceId = id });
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var dbResource = B.GetResource(id);
            var dbCompetence = B.GetCompetenceFromResourceId(id);
            if (dbResource == null ||
                dbCompetence == null)
                return RedirectToAction("Index", "Home");

            ResourceViewModel vm = new()
            {
                ResourceId = dbResource.Id,
                CompetenceId = dbCompetence.Id,
                DisplayText = dbResource.DisplayText,
                Link = dbResource.Link
            };

            return View("Resource", vm);
        }

        [HttpPost]
        public IActionResult Edit(ResourceViewModel vm)
        {
            if (!ModelState.IsValid)
                return View("Resource", vm);

            var competence = B.GetCompetence(vm.CompetenceId);
            if (competence == null)
                return RedirectToAction("Index", "Home");

            Resource resource = new()
            {
                Id = vm.ResourceId,
                DisplayText = vm.DisplayText,
                Link = vm.Link,
                Competence = competence
            };

            if (resource.Id == 0)
            {
                B.CreateResource(resource);
            }
            else
            {
                B.UpdateResource(resource);
            }

            return RedirectToAction("Index", new { id = vm.CompetenceId });
        }

        public IActionResult Delete(int id)
        {
            var competence = B.GetCompetenceFromResourceId(id);
            B.DeleteResource(id);
            return RedirectToAction("Index", new { id = competence?.Id });
        }
    }
}
