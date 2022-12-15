using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using BL.Models;
using Kompetenzverwaltung.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Kompetenzverwaltung.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ManagementController : Controller
    {
        private UserManager<ApplicationUser> _userManager;

        public ManagementController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Users()
        {
            var model = new List<UserViewModel>();
            foreach (var user in _userManager.Users)
            {
                model.Add(new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.UserName,
                    IsAdministrator = (await _userManager.GetRolesAsync(user).ConfigureAwait(false)).Contains("Administrator")
            });
            }
            return View(model);
        }

        public async Task<IActionResult> CreateUser(string id)
        {
            var model = new UserViewModel();
            if (id != null)
            {
                var user = await _userManager.FindByIdAsync(id);
                model.Id = user.Id;
                model.Name = user.UserName;
                model.Email = user.Email;
                model.IsAdministrator = (await _userManager.GetRolesAsync(user).ConfigureAwait(false)).Contains("Administrator");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserViewModel user)
        {
            ModelState.Remove("Id");

            if (user.Id != null)
            {
                if(user.Password != null)
                    ModelState.Remove("Password");

                if (ModelState.IsValid)
                {
                    var appUser = await _userManager.FindByIdAsync(user.Id);
                    if(user.Password != null)
                    {
                        var token = await _userManager.GeneratePasswordResetTokenAsync(appUser);
                        await _userManager.ResetPasswordAsync(appUser, token, user.Password);
                    }
                    appUser.UserName = user.Name;
                    appUser.Email = user.Email;
                    if (user.IsAdministrator)
                    {
                        await _userManager.AddToRoleAsync(appUser, "Administrator");
                    }
                    else
                    {
                        await _userManager.RemoveFromRoleAsync(appUser, "Administrator");
                    }
                    IdentityResult result = await _userManager.UpdateAsync(appUser);
                    if (result.Succeeded)
                        return RedirectToAction("Users");
                    else
                        return View(user);
                }
            }

            if (ModelState.IsValid)
            {
                ApplicationUser appUser = new ApplicationUser
                {
                    UserName = user.Name,
                    Email = user.Email
                };

                IdentityResult result = await _userManager.CreateAsync(appUser, user.Password);

                if (user.IsAdministrator)
                {
                    await _userManager.AddToRoleAsync(appUser, "Administrator");
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(appUser, "Administrator");
                }

                if (result.Succeeded)
                    return RedirectToAction("Users");
                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var result = await _userManager.DeleteAsync(user);
            return Json(result);
        }
    }
}
