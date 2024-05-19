using CrimeManagement.Data;
using CrimeManagement.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CrimeManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //get all users
            var users = await userManager.Users.ToListAsync();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            ViewBag.UserName = user.UserName;

            var userRoles = await userManager.GetRolesAsync(user);

            return View(userRoles);
        }

        [HttpGet]
        public IActionResult AddRole()
        {
        
            return RedirectToAction( nameof(DisplayRoles));
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(string role)
        {
            //create new role using RoleManager
            await roleManager.CreateAsync(new IdentityRole(role));
            return RedirectToAction(nameof(DisplayRoles));
        }

        [HttpGet]
        public async Task<IActionResult> DisplayRoles()
        {
            //getAll roles to view
            var roles = await roleManager.Roles.ToListAsync();
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> AddUserToRole()
        {
            //getAll roles and users
            var users = await userManager.Users.ToListAsync();
            var roles = await roleManager.Roles.ToListAsync();

            ViewBag.Users = new SelectList(users, "Id", "UserName");
            ViewBag.Roles = new SelectList(roles, "Name", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToRole(UserRole userRole)
        {
            //find user and assign role 
            var user = await userManager.FindByIdAsync(userRole.UserId);
            await userManager.AddToRoleAsync(user, userRole.RoleName);


            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> RemoveUserRole(string role, string userName)
        {
            //remove role from user
            var user = await userManager.FindByNameAsync(userName);
            var result = await userManager.RemoveFromRoleAsync(user, role);


            return RedirectToAction(nameof(Details), new {userId = user.Id});
        }

        [HttpGet]
        public async Task<IActionResult> RemoveRole(string role)
        {
            //remove role from Roles
            var roleToDelete = await roleManager.FindByNameAsync(role);
            var result = await roleManager.DeleteAsync(roleToDelete);


            return RedirectToAction(nameof(DisplayRoles));
        }
    }
}
