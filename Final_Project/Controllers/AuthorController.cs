using Final_Project.Models;
using Final_Project.Reposatiory;
using Final_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Final_Project.Controllers
{
    public class AuthorController : Controller
    {
        AuthorRepository authorReposatiory;
        BookReposatiory bookReposatiory;
        CategoryRepository categorieReposatiory;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment webHostEnvironment;

        public AuthorController(
            AuthorRepository authorRepo,
            UserManager<ApplicationUser> userManager,
            BookReposatiory bookRebo,
            IWebHostEnvironment webHostEnvironment,
            CategoryRepository categorieReposatiory

            )
        {
            authorReposatiory = authorRepo;
            bookReposatiory = bookRebo;
            _userManager = userManager;
            this.webHostEnvironment = webHostEnvironment;
            this.categorieReposatiory = categorieReposatiory;
        }
        // get all books of the author that is entered 
        //public IActionResult Index()
        //{

        //    return View();
        //}
        [Authorize(Roles = "Author")]
        public async Task<IActionResult> getBooksAuthor()
        {
            var user = await _userManager.GetUserAsync(User);
            var books = bookReposatiory.Info(user.Id);
            
            return View(books);
        }
        [Authorize(Roles = "Author")]
        public IActionResult AddBook()
        {
            List<Categorie> c1 = categorieReposatiory.GetCategories();
            var send_data = new AddBook()
            {
                Cat = c1
            };
            return View(send_data);

        }
        [Authorize(Roles = "Author")]
        [HttpPost]
        public async Task<IActionResult> AddBook(AddBook model, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                var book =new Book();
                book.Name = model.Name;
                book.categories_Id = model.categories_Id;
                book.Description = model.Description;
                book.Salary = model.Salary;
                book.Title = model.Title;
                book.Author_Id = model.Author_Id;
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
                    book.Photo = fileName;
                }


                bookReposatiory.Insert(book);
                return RedirectToAction("getBooksAuthor");
            }
            List<Categorie> c1 = categorieReposatiory.GetCategories();
            var send_data = new AddBook()
            {
                Cat = c1
            };
            return View(send_data);

        }
        [Authorize(Roles = "Author")]
        public IActionResult Edit_Book(string id)
        {
            var book = bookReposatiory.GetBook(id);
            List<Categorie> c1 = categorieReposatiory.GetCategories();
            var send_data = new AddBook()
            {
                Id=book.Id,
                Name=book.Name,
                categories_Id=book.categories_Id,
                Description=book.Description,
                Salary=book.Salary,
                Title=book.Title,
                Author_Id=book.Author_Id,
                Photo=book.Photo,
                Cat = c1
            };

            return View(send_data);
        }
        [Authorize(Roles = "Author")]
        [HttpPost]

        public async Task<IActionResult> Edit_Book(AddBook model, IFormFile? imageFile)
        {
            if (imageFile == null)
            {
                var bookCheckPhoto = bookReposatiory.GetBook(model.Id);
                if (bookCheckPhoto != null)
                {
                    model.Photo = bookCheckPhoto.Photo;
                }

            }
            if (ModelState.IsValid)
            {
                var book = new Book();
                book.Id = model.Id;
                book.Name = model.Name;
                book.categories_Id = model.categories_Id;
                book.Description = model.Description;
                book.Salary = model.Salary;
                book.Title = model.Title;
                book.Author_Id = model.Author_Id;
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
                    book.Photo = fileName;
                }
                 bookReposatiory.Update(book.Id,book);
                return RedirectToAction("getBooksAuthor");
            }
            List<Categorie> c1 = categorieReposatiory.GetCategories();
            var send_data = new AddBook()
            {
                Cat = c1
            };
            return View(send_data);
        }
        [Authorize(Roles = "Author")]
        public IActionResult Details_books(string id)
        {
            var data=bookReposatiory.GetBook(id);
            return View(data);
        }
        [Authorize(Roles = "Author")]
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
                Address = user.Address,
                Phone = user.PhoneNumber,
                Name = user.UserName,
                Role = roles.FirstOrDefault(),
                Photo = user.Photo
            };


            return View(model);
        }
        [Authorize(Roles = "Author")]
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
                Address = user.Address,
                Phone = user.PhoneNumber,
                Name = user.UserName,
                Role = roles.FirstOrDefault(),
                Photo = user.Photo
            };
            return View(model);
        }
        [Authorize(Roles = "Author")]
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
                user.Address = model.Address;
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
                    return RedirectToAction("Profile", "Author", new { id = user.Id });
                }
            }


            return View(model);
        }
        [HttpGet]
        public IActionResult Delete(string id)
        {
            
            return View(bookReposatiory.GetBook(id));
        }
        [HttpPost]
        public IActionResult Delete_book(string id)
        {
            bookReposatiory.Delete(id);
            return RedirectToAction(nameof(getBooksAuthor));
        }



    }

}
