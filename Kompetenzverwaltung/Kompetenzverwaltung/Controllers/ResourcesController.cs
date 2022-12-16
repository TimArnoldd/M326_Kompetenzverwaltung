using BL;
using BL.Models;
using Kompetenzverwaltung.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kompetenzverwaltung.Controllers
{
    public class ResourcesController : Controller
    {
        private readonly B b;
        public ResourcesController()
        {
            b = new();
        }

        [HttpGet]
        public IActionResult Index(int id)
        {
            if (id == 0)
                return RedirectToAction("Index", "Home");
            var resources = b.GetResourcesFromCompetence(id);
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
            var dbResource = b.GetResource(id);
            var dbCompetence = b.GetCompetenceFromResourceId(id);
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

            var competence = b.GetCompetence(vm.CompetenceId);
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
                b.CreateResource(resource);
            }
            else
            {
                b.UpdateResource(resource);
            }

            return RedirectToAction("Index", new { id = vm.CompetenceId });
        }

        public IActionResult Delete(int id)
        {
            var competence = b.GetCompetenceFromResourceId(id);
            b.DeleteResource(id);
            return RedirectToAction("Index", new { id = competence?.Id });
        }
    }
}
