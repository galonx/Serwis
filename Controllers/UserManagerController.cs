using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serwis.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Serwis.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserManagerController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserManagerController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["IdSortParam"] = string.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewData["EmailSortParam"] = sortOrder == "email" ? "email_desc" : "email";
            ViewData["UserNameSortParam"] = sortOrder == "username" ? "username_desc" : "username";
            ViewData["RoleSortParam"] = sortOrder == "role" ? "role_desc" : "role";

            var users = _userManager.Users.ToList();
            var userList = new List<UserManagerModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("Admin")) continue;

                userList.Add(new UserManagerModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    Roles = roles.ToList()
                });
            }

            userList = sortOrder switch
            {
                "id_desc" => userList.OrderByDescending(u => u.Id).ToList(),
                "email" => userList.OrderBy(u => u.Email).ToList(),
                "email_desc" => userList.OrderByDescending(u => u.Email).ToList(),
                "username" => userList.OrderBy(u => u.UserName).ToList(),
                "username_desc" => userList.OrderByDescending(u => u.UserName).ToList(),
                "role" => userList.OrderBy(u => string.Join(", ", u.Roles)).ToList(),
                "role_desc" => userList.OrderByDescending(u => string.Join(", ", u.Roles)).ToList(),
                _ => userList.OrderBy(u => u.Id).ToList(),
            };

            return View(userList);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Admin"))
            {
                return RedirectToAction("Index");
            }

            var model = new UserManagerModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Roles = roles.ToList()
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Admin"))
            {
                return RedirectToAction("Index");
            }

            await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AddRole(string id, string role)
        {
            if (role == "Admin") return RedirectToAction("Index");

            var user = await _userManager.FindByIdAsync(id);
            if (user != null && !await _userManager.IsInRoleAsync(user, role))
            {
                await _userManager.AddToRoleAsync(user, role);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveRole(string id, string role)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null && await _userManager.IsInRoleAsync(user, role))
            {
                await _userManager.RemoveFromRoleAsync(user, role);
            }
            return RedirectToAction("Index");
        }
    }
}
