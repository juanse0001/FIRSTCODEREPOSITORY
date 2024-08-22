using BibliotecaWebApplication.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;

namespace BibliotecaWebApplication.Controllers
{
    [Authorize(Roles = "Root, Administrador")]
    public class UserRolesController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRolesController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        //GET : UserRolesController
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var userRolesViewModel = new List<UserRolesViewModel>();

            foreach (var user in users)
            {
                var thisViewModel = new UserRolesViewModel
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Roles = await GetUserRoles(user)
                };
                userRolesViewModel.Add(thisViewModel);
            }

            return View(userRolesViewModel);
        }
        private async Task<List<string>> GetUserRoles(IdentityUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }


        // GET: UserRolesController/Manage
        public async Task<IActionResult> Manage(string userId)
        {
            ViewBag.UserId = userId;
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            ViewBag.UserName = user.UserName;

            var model = new List<ManageUserRolesViewModel>();

            foreach (var role in _roleManager.Roles.ToList())
            {
                var userRolesViewModel = new ManageUserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    IsSelected = await _userManager.IsInRoleAsync(user, role.Name)
                };
                model.Add(userRolesViewModel);
            }

            return View(model);
        }


        //POST: UserRolesController/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Manage(List<ManageUserRolesViewModel> roles, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return View("NotFound");
            }

            var currentRoles = await _userManager.GetRolesAsync(user);

            var rolesToAdd = roles.Where(x => x.IsSelected && !currentRoles.Contains(x.RoleName)).Select(x => x.RoleName);
            var rolesToRemove = currentRoles.Where(role => roles.Any(x => x.RoleName == role && !x.IsSelected));

            var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles: " + string.Join(", ", removeResult.Errors.Select(e => e.Description)));
                return View(roles);
            }

            var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user: " + string.Join(", ", addResult.Errors.Select(e => e.Description)));
                return View(roles);
            }

            return RedirectToAction("Index");
        }

    }
}