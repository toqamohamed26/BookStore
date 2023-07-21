using Final_Project.Models;
using Final_Project.Reposatiory;
using Final_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Final_Project.Controllers
{
    [Authorize (Roles ="Admin")]
    public class CategoryController : Controller
    {
        private CategoryRepository caterepo;
        public CategoryController(CategoryRepository _caterepo)
        {
            caterepo = _caterepo;
        }
        public IActionResult Index()
        {
            List<Categorie> cat = caterepo.GetCategories();
            return View(cat);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Categorie cat)
        {
            if (cat != null)
            {
                caterepo.Insert(cat);
                caterepo.Save();
                return RedirectToAction("Index");
            }

            return View("Create", cat);
        }

        public IActionResult Details(string id)
        {
            Categorie catbyid = caterepo.GetById(id);
            return View(catbyid);
        }
        [HttpGet]
        public IActionResult Edit(string id)
        {
            Categorie catbyid = caterepo.GetById(id);
            return View(catbyid);
        }
        [HttpPost]
        public IActionResult Edit(Categorie cat, string id)
        {
            if (cat != null)
            {
                caterepo.Update(id, cat);
                caterepo.Save();
                return RedirectToAction("Index");
            }
            return View(cat);
        }

        public IActionResult Delete(string id)
        {

            caterepo.Delete(id);
            caterepo.Save();
            return RedirectToAction("Index");
        }
        public IActionResult Search(string searchValue)
        {
            var filteredUsers = caterepo.SearchCategories(searchValue);

            
            return PartialView("_CatogriesTablePartial", filteredUsers); // Assuming you have a partial view for the table body
        }

        // GET: /User/ResetSearch
        public IActionResult ResetSearch()
        {
            var allUsers = caterepo.GetCategories();

            
            return PartialView("_CatogriesTablePartial", allUsers); // Assuming you have a partial view for the table body
        }





    }
}
