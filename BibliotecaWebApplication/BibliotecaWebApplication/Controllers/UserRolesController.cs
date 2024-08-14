using BibliotecaWebApplication.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;

namespace BibliotecaWebApplication.Controllers
{
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
                var thisViewModel = new UserRolesViewModel();
                thisViewModel.UserId = user.Id;
                thisViewModel.Email = user.Email;
                thisViewModel.Roles = await GetUserRoles(user);
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
                return View("Not Found");
            }
            ViewBag.UserName = user.UserName;

            var model = new List<ManageUserRolesViewModel>();

            foreach (var role in _roleManager.Roles.ToList())
            {
                var userRolesViewModel = new ManageUserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }
                model.Add(userRolesViewModel);
            }
            return View(model);
        }
        //POST: UserRolesController/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Manage(List<ManageUserRolesViewModel> model, string userId)
        {
            //Encuentra al usuario basado en el userId proporcionado
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Not Found");
            }

            //Obten los roles acutales asociados al usuario
            var currentRoles = await _userManager.GetRolesAsync(user);

            //Filtro para obtener los roles actaules asociados al usuario
            var rolesToAdd = model.Where(x => x.IsSelected && !currentRoles.Contains(x.RoleName)).Select(x => x.RoleName);

            //Filtro para obtener los roles que necesitan ser removidos
            var rolesToRemove = currentRoles.Where(role => !model.Any(x => x.RoleName == role && x.IsSelected));

            //Resultado de remover los roles no deseados
            var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }

            //Resultado de agregar los nuevos roles seleccionados
            var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }

            //Redirige al indice si todo es exitoso
            return RedirectToAction("Index");
        }
    }
}