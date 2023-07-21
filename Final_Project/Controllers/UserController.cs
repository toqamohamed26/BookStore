using Final_Project.Models;
using Final_Project.Reposatiory;
using Final_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System.Data;

namespace Final_Project.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        RoleManager<IdentityRole> _roleManager;
        AuthorRepository _authorRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UserController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
             AuthorRepository authorRepository,
             IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _authorRepository = authorRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            var model = users.Select(u => new UsersDetailsViewModel()
            {
                id = u.Id,
                Email = u.Email,
                Addrss = u.Address,
                Phone = u.PhoneNumber,
                Name = u.UserName,
                Role = _userManager.GetRolesAsync(u).Result.FirstOrDefault(),
                Photo = u.Photo,
                
            }).ToList();

            return View(model);
        }
        public IActionResult AddUser()
        {
            var viewmodel = new RegisterViewModel()
            {
                RoleOptions = _roleManager.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList()
            };
            return View(viewmodel);
        }
        [HttpPost]
        public async Task< IActionResult> AddUser(RegisterViewModel model, IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                string fileName = Path.GetFileName(imageFile.FileName);

                // Specify the path where the image will be saved
                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);

                // Save the image file to the specified path
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }

                // Set the Image property of the Instructor object
                model.Photo = fileName;

                if (ModelState.IsValid)
                {
                    ApplicationUser user = new ApplicationUser()
                    {
                        Email = model.Email,
                        Address = model.Addrss,
                        PhoneNumber = model.Phone,
                        UserName = model.Name,
                        Photo = model.Photo
                    };
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {

                        var selectedRole = await _roleManager.FindByNameAsync(model.Role);
                        if (selectedRole != null)
                        {
                            await _userManager.AddToRoleAsync(user, selectedRole.Name);
                            if (model.Role == "Author")
                            {
                                var author = new Author()
                                {
                                    Id = user.Id,
                                    Name = model.Name,
                                    Email = model.Email,
                                    Password = model.Password,
                                    Photo = model.Photo
                                };
                                _authorRepository.Insert(author);


                            }
                            return RedirectToAction(nameof(GetAllUsers));
                        }
                    }
                    else
                    {

                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }

                }
            }
            model.RoleOptions = _roleManager.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList();
            return View(model);
        }
        public async Task<IActionResult> UpdateUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var model = new UsersDetailsViewModel()
            {
                id = user.Id,
                Email = user.Email,
                Addrss = user.Address,
                Phone = user.PhoneNumber,
                Name = user.UserName,
                Role = roles.FirstOrDefault(),
                Photo = user.Photo
            };
            model.RoleOptions = _roleManager.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UsersDetailsViewModel model, IFormFile? imageFile)
        {
            if(imageFile == null)
            {
                var user = await _userManager.FindByIdAsync(model.id);
                if (user != null)
                {
                    model.Photo = user.Photo;
                }

            }
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.id);
                if (user == null)
                {
                    return NotFound();
                }

                user.Email = model.Email;
                user.Address = model.Addrss;
                user.PhoneNumber = model.Phone;
                user.UserName = model.Name;

                if (imageFile != null && imageFile.Length > 0)
                {
                    string fileName = Path.GetFileName(imageFile.FileName);

                    // Specify the path where the image will be saved
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);

                    // Save the image file to the specified path
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    // Set the Image property of the Instructor object
                    user.Photo = fileName;
                }


                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, roles);
                    await _userManager.AddToRoleAsync(user, model.Role);
                    if (model.Role == "Author")
                    {
                        var author = new Author()
                        {
                            Id = user.Id,
                            Name = model.Name,
                            Email = model.Email,
                            Photo = model.Photo
                        };
                        _authorRepository.Update(author.Id,author);


                    }
                    return RedirectToAction("GetAllUsers");
                }
            }

            model.RoleOptions = _roleManager.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList();
            return View(model);
        }


        public async Task<IActionResult> Search(string searchValue)
        {
            var filteredUsers = await _userManager.Users
                .Where(u => u.UserName.Contains(searchValue) || u.Email.Contains(searchValue))
                .ToListAsync();

            var viewModel = MapToViewModel(filteredUsers);
            return PartialView("_UserTablePartial", viewModel); // Assuming you have a partial view for the table body
        }

        // GET: /User/ResetSearch
        public async Task<IActionResult> ResetSearch()
        {
            var allUsers = await _userManager.Users.ToListAsync();

            var viewModel = MapToViewModel(allUsers);
            return PartialView("_UserTablePartial", viewModel); // Assuming you have a partial view for the table body
        }

        private IEnumerable<UsersDetailsViewModel> MapToViewModel(IEnumerable<ApplicationUser> users)
        {
            var viewModel = users.Select(u => new UsersDetailsViewModel
            {
                id = u.Id,
                Email = u.Email,
                Addrss = u.Address,
                Phone = u.PhoneNumber,
                Name = u.UserName,
                Role = _userManager.GetRolesAsync(u).Result.FirstOrDefault(),
                Photo = u.Photo,
            });

            return viewModel;
        }
    }
}
