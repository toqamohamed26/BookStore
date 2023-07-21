using Final_Project.Models;

namespace Final_Project.Reposatiory
{
    public class CategoryRepository : ICategorieReposatiory
    {
        BookStoreContext db;

        public CategoryRepository(BookStoreContext _db)
        {
            db = _db;

        }
        public void Delete(string id)
        {
            Categorie oldcate = GetById(id);
            //oldcate.IsDeleted = true; //soft delete
        }

        public Categorie GetCategorie(string id)
        {
            return db.Categories.FirstOrDefault(a => a.Id == id);
        }

        public List<Categorie> GetCategories()
        {
            return db.Categories.ToList();
        }

        public void Insert(Categorie categorie)
        {
            db.Categories.Add(categorie);
            Save();
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(string id, Categorie categorie)
        {
            //Categorie oldcate = db.Categories.FirstOrDefault(c=>c.Id==id);
            Categorie oldcate = GetById(id);

            oldcate.Name = categorie.Name;
            oldcate.Description = categorie.Description;
            Save();


        }
        public Categorie GetById(string id)
        {
            return db.Categories.FirstOrDefault(d => d.Id == id);// &d.IsDeleted==false);
        }

        public List<Categorie> SearchCategories(string searchValue)
        {
            var filteredCategories = db.Categories
                .Where(c => c.Name.Contains(searchValue))
                .ToList();

            return filteredCategories;
        }




    }
}
