using Final_Project.Models;
using Final_Project.Reposatiory;
using Final_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Final_Project.Controllers
{
    [Authorize(Roles = "User")]
    public class EnterUserController : Controller
    {
        IAuthorRepository authorReposatiory;
        BookReposatiory bookReposatiory;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment webHostEnvironment;

        public EnterUserController(
            IAuthorRepository authorRepo,
            UserManager<ApplicationUser> userManager,
            BookReposatiory bookRebo,
            IWebHostEnvironment webHostEnvironment

            )
        {
            authorReposatiory = authorRepo;
            bookReposatiory = bookRebo;
            _userManager = userManager;
            this.webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Home_User()
        {
            return View("Index","Home");
        }
        public IActionResult DisplayBooks()
        {
            var data = bookReposatiory.GetBooks();
            return View(data);
        }
        public async Task<IActionResult> Profile(string id)
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
            
            
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
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
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UsersDetailsViewModel model, IFormFile? imageFile)
        {
            if (imageFile == null)
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
                    string imagePath = Path.Combine(webHostEnvironment.WebRootPath, "images", fileName);

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
                        authorReposatiory.Update(author.Id, author);


                    }
                    return RedirectToAction("Profile","EnterUser", new { id = user.Id });
                }
            }

            
            return View(model);
        }
    }
}
