using Final_Project.Models;
using Final_Project.Reposatiory;
using Final_Project.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Final_Project.Controllers
{
    public class HomeController : Controller

    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AuthorRepository _authorRepository;
        

        public HomeController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            AuthorRepository authorRepository,
            IWebHostEnvironment webHostEnvironment
            )
        {
            _userManager=userManager;
            _signInManager=signInManager;
            _roleManager=roleManager;
            _authorRepository=authorRepository;
            _webHostEnvironment = webHostEnvironment;

        }

        [HttpGet]
        public IActionResult logIn()
        {


            return View();

        }
        [HttpPost]
        public async Task<IActionResult> logIn(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {


                var data = await _signInManager.PasswordSignInAsync(model.UserName, model.Password,isPersistent:false, lockoutOnFailure: false);
                if (data.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.UserName);

                    if (user != null)
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                        var role =roles.FirstOrDefault();
                            if (role == "Author")
                            {
                                return RedirectToAction(nameof(Index));
                                //return to Author action 
                            }
                            if (role == "Admin")
                            {
                                return RedirectToAction("Home_Admin","Admin");
                        }
                            if (role == "User")
                            {
                            return RedirectToAction("DisplayBooks", "EnterUser");
                        }


                    }

                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");

            }

            return View(model);

        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            var viewmodel = new RegisterViewModel()
            {
                RoleOptions = _roleManager.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).Take(2).ToList()
            };
            return View(viewmodel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model, IFormFile imageFile)
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
                        Address = model.Address,
                        PhoneNumber = model.Phone,
                        UserName = model.Name,
                        Photo=model.Photo
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
                            return RedirectToAction(nameof(logIn));
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
            model.RoleOptions = _roleManager.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).Take(2).ToList();
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("login", "Home");
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult ContactUs()
        {
            return View();
        }
    }
}