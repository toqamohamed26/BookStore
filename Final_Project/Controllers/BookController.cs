//using Final_Project.Models;
//using Final_Project.Reposatiory;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using System.Data;

//namespace Final_Project.Controllers
//{
//    public class BookController : Controller
//    {
//        IAuthorRepository authorReposatiory;
//        IBookReposatiory bookReposatiory;
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly IWebHostEnvironment webHostEnvironment;

//        public BookController(
//            IAuthorRepository authorRepo,
//            UserManager<ApplicationUser> userManager,
//            IBookReposatiory bookRebo,
//            IWebHostEnvironment webHostEnvironment

//            )
//        {
//            authorReposatiory = authorRepo;
//            bookReposatiory = bookRebo;
//            _userManager = userManager;
//            this.webHostEnvironment = webHostEnvironment;
//        }




//        [Authorize(Roles = "User")]

//        public IActionResult getAll()
//        {
//            return View(bookReposatiory.GetBooks());
//        }

//        public IActionResult HisInfo()
//        {
//            var userId = _userManager.GetUserId(User);
//            return View(bookReposatiory.Info(userId));

//        }

//        [Authorize(Roles = "User")]
//        // meaning books of user 
//        public IActionResult DashBoard()
//        {
//            return View(bookReposatiory.GetBooks());
//        }

//        // ************************************function wihout any thing and any data ******************************************
//        //public IActionResult MainDash()
//        //{
//        //    return View();
//        //}

//        //public IActionResult getbookById(string id)
//        //{
//        //    return View(authorReposatiory.GetAuthor(id));
//        //}

//        //[HttpGet]
//        //public IActionResult Create()
//        //{
//        //    ViewData["authList"] = authorReposatiory.GetAuthors();

//        //    return View();
//        //}

//        //[HttpPost]
//        //[ValidateAntiForgeryToken]
//        //public async Task<IActionResult> Create(Book book, IFormFile Photo)
//        //{
//        //    if (Photo == null || Photo.Length == 0)
//        //    {
//        //        return Content("File not selected");
//        //    }
//        //    string FileName = Path.GetFileName(Photo.FileName);
//        //    var path = Path.Combine(webHostEnvironment.WebRootPath, "images", Photo.FileName);
//        //    using (FileStream stream = new FileStream(path, FileMode.Create))
//        //    {
//        //        await Photo.CopyToAsync(stream);
//        //        stream.Close();
//        //    }
//        //    book.Photo = FileName;

//        //    if (ModelState.IsValid == true)
//        //    {
//        //        bookReposatiory.Insert(book);
//        //        return RedirectToAction("getAll");
//        //    }
//        //    else
//        //    {
//        //        ViewData["authList"] = authorReposatiory.GetAuthors();

//        //        return View(book);
//        //    }

//        //}

//        //[HttpGet]
//        //public IActionResult Edit(string Id)
//        //{
//        //    Book book = bookReposatiory.GetBook(Id);
//        //    ViewData["authList"] = authorReposatiory.GetAuthors();
//        //    return View(book);
//        //}

//        //[HttpPost]
//        //[ValidateAntiForgeryToken]
//        //public async Task<IActionResult> Edit(Book book, string id, IFormFile Photo)
//        //{
//        //    if (Photo == null || Photo.Length == 0)
//        //    {
//        //        return Content("File not selected");
//        //    }
//        //    string FileName = Path.GetFileName(Photo.FileName);
//        //    var path = Path.Combine(webHostEnvironment.WebRootPath, "images", Photo.FileName);
//        //    using (FileStream stream = new FileStream(path, FileMode.Create))
//        //    {
//        //        await Photo.CopyToAsync(stream);
//        //        stream.Close();
//        //    }
//        //    book.Photo = FileName;

//        //    if (ModelState.IsValid == true)
//        //    {
//        //        bookReposatiory.Update(id, book);
//        //        return RedirectToAction("getAll");
//        //    }
//        //    ViewData["authList"] = authorReposatiory.GetAuthors();
//        //    return View(book);
//        //}
//        //public IActionResult Delete(string id)
//        //{
//        //    bookReposatiory.Delete(id);
//        //    return RedirectToAction("getAll");
//        //}
//        //public IActionResult Search(string Name)
//        //{
//        //    if (Name != "")
//        //    {
//        //        bookReposatiory.Searchh(Name);
//        //        return View("getall");
//        //    }
//        //    else
//        //    {
//        //        bookReposatiory.Search(Name);

//        //        return View("getall");
//        //    }

//        //}
//    }
//}
