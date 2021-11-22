using _1_app.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace _1_app.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)

        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users
               //.Where(e => e.UserName != "Admin")
               .Select(user => new UserViewModel()
               {
                   Id = user.Id,
                   FirstName = user.FirstName,
                   LastName = user.LastName,
                   Email = user.Email,
                   Username = user.UserName,
                   Roles = _userManager.GetRolesAsync(user).Result
               }).ToListAsync();


            /********/



            //if (User.FindFirst(ClaimTypes.Role).Value == "Teacher")
            if (User.IsInRole("Teacher") && !User.IsInRole("Admin"))
            {
                users = users.Where(e => e.Roles.ToList().Contains("student")).ToList();
            }
            /********/

            return View(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Add(string userId)
        {
            var roles = await _roleManager.
                Roles.Select(r => new RoleViewModel()
                {
                    RoleId = r.Id,
                    RoleName = r.Name
                })
                .ToListAsync();

            var ViewModel = new AddUsersViewModel
            {
                Roles = roles
            };

            return View(ViewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddUsersViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (!model.Roles.Any(role => role.IsSelected))
            {
                ModelState.AddModelError("Roles", "Please select at least one role");
                return View(model);
            }
            if (!string.IsNullOrEmpty(model.Email?.Trim()))
                if (await _userManager.FindByEmailAsync(model.Email) != null)
                {
                    ModelState.AddModelError("Email", "Email is Already Exists");
                    return View(model);
                }

            if (await _userManager.FindByNameAsync(model.Username) != null)
            {
                ModelState.AddModelError("Username", "Username is Already Exists");
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Username,
                //new MailAddress(Input.Email).User;
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Roles", error.Description);
                }
                return View(model);
            }
            await _userManager.AddToRolesAsync(user, model.Roles
                    .Where(r => r.IsSelected)
                    .Select(r => r.RoleName));

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> ManageRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            var roles = await _roleManager.Roles.ToListAsync();
            var ViewModel = new UserRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles.Select(role => new RoleViewModel
                {
                    RoleName = role.Name,
                    RoleId = role.Id,
                    IsSelected = _userManager.IsInRoleAsync(user, role.Name).Result
                }).ToList()
            };

            return View(ViewModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageRole(UserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }
            var usersRole = await _userManager.GetRolesAsync(user);
            foreach (var role in model.Roles)
            {
                if (usersRole.Any(r => r == role.RoleName) &&
                    !role.IsSelected)
                {
                    await _userManager.RemoveFromRoleAsync(user, role.RoleName);
                }

                if (!usersRole.Any(r => r == role.RoleName) &&
                     role.IsSelected)
                {
                    await _userManager.AddToRoleAsync(user, role.RoleName);
                }

            }
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> ManageUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var ViewModel = new UserProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.UserName,
                Email = user.Email,
                Id = user.Id
            };

            return View(ViewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ManageUser(UserProfileViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                return NotFound();
            }

            //check email
            var userEmail = await _userManager.FindByEmailAsync(model.Email); ;
            if (!string.IsNullOrEmpty(model.Email?.Trim()))
                if (userEmail != null)
                    if (userEmail.Id != model.Id)
                    {
                        ModelState.AddModelError("Email", "Email is Already Exists");
                        return View(model);
                    }

            //check username
            var userUsername = await _userManager.FindByNameAsync(model.Username); ;
            if (userUsername != null)
                if (userUsername.Id != model.Id)
                {
                    ModelState.AddModelError("Username", "Username is Already Exists");
                    return View(model);
                }

            if (model.Password != null && model.ConfirmPassword != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
            }
            //edit user
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UserName = model.Username;

            await _userManager.UpdateAsync(user);


            return RedirectToAction(nameof(Index));
        }

    }
}
