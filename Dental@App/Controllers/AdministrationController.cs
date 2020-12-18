using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dental_App.DataContext;
using Dental_App.Models;
using Dental_App.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dental_App.Controllers
{
    // Tylko użytkownik pełniący rolę Admin ma dostęp do funkcji Administratora
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
        {
            private readonly RoleManager<IdentityRole> _roleManager;
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly ApplicationDbContext _context;

            public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager,
                ApplicationDbContext context)
            {
                _roleManager = roleManager;
                _userManager = userManager;
                _context = context;
            }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                IdentityResult result = await _roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }

                //added .ToList()
                foreach (IdentityError error in result.Errors.ToList())
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [HttpPost]
            public async Task<IActionResult> DeleteUser(string id)
            {
                // znajdź usera po Id
                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                {
                    ViewBag.Errormessage = $"User z ID = {id} nie został znaleziony.";
                    return View("NotFound");
                }

             
                else
                {
                    var result = await _userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListUsers");
                    }

                    //added .ToLIst()
                    foreach (var error in result.Errors.ToList())
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View("ListUsers");
                }
            }

            [HttpPost]
            public async Task<IActionResult> DeleteRole(string id)
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                {
                   
                    ViewBag.ErrorMessage = $"Rola z ID = {id} nie została znaleziona.";
                    return View("NotFound");
                }

                else
                {
                    var result = await _roleManager.DeleteAsync(role);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRoles");
                    }

                    //added .ToList()
                    foreach (var error in result.Errors.ToList())
                    {

                        ModelState.AddModelError("", error.Description);
                    }
                    return View("ListRoles");
                }
            }
        
        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = _roleManager.Roles;

            return View(roles);
        }

        public IActionResult ListUsers()
        {
            var users = _userManager.Users;
            return View(users);
        }


        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.Errormessage = $"User z ID = {id} nie został znaleziony.";
                return View("NotFound");
            }

            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);


            var model = new EditUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Pesel = user.PeselID,
                Email = user.Email,
                UserName = user.UserName,
                Claims = userClaims.Select(c => c.Value).ToList(),
                Roles = userRoles
            };
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ViewBag.Errormessage = $"User z ID = {model.Id} nie został znaleziony.";
                return View("NotFound");
            }

            else
            {
                // kopiuję UserName, Imie, Nazwisko, Pesel z obiektu model do odpowiednich propertisów w obiekcie user
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.PeselID = model.Pesel;


                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }

                //added .ToList()
                foreach (var error in result.Errors.ToList())
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            };
        }


        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.Errormessage = $"Rola z ID = {id} nie została znaleziona.";
                return View("NotFound");
            }

            var model = new EditRoleViewModel
            {
                RoleID = role.Id,
                RoleName = role.Name
            };

            //added .ToList()
            foreach (var user in _userManager.Users.ToList())
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleID);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Rola z ID = {model.RoleID} nie została znaleziona.";

                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                //added .ToList()
                foreach (var error in result.Errors.ToList())
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Rola z ID = {roleId} nie została znaleziona.";

                return View("NotFound");
            }

            var model = new List<UserRoleViewModel>();

            //added .ToList()
            foreach (var user in _userManager.Users.ToList())
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserID = user.Id,
                    UserName = user.UserName
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }

                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Rola z ID = {roleId} nie została znaleziona.";

                return View("NotFound");
            }
            for (int i = 0; i < model.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model[i].UserID);

                IdentityResult result = null;

                if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                
                else if (!model[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }
                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))

                        continue;
                    else

                        return RedirectToAction("EditRole", new { Id = roleId });
                }

            }

            return RedirectToAction("EditRole", new { Id = roleId });
        }

        // List patient
        [HttpGet]
        public async Task<IActionResult> ListPatients(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.Errormessage = $"Rola z ID = {id} nie została znaleziona.";
                return View("NotFound");
            }

            var model = new EditRoleViewModel
            {
                RoleID = role.Id,
                RoleName = role.Name
            };
            
            //added .ToList()
            foreach (var user in _userManager.Users.ToList())
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ListPatients(EditRoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleID);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Rola z ID = {model.RoleID} nie została znaleziona.";

                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                //added .ToList()
                foreach (var error in result.Errors.ToList())
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }
    }
}

