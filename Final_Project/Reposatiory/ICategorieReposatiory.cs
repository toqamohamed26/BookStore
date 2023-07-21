using Final_Project.Models;

namespace Final_Project.Reposatiory
{
    public interface ICategorieReposatiory
    {
        List<Categorie> GetCategories();

        Categorie GetCategorie(string id);
        Categorie GetById(string id);
        void Insert(Categorie categorie);
        void Update(string id, Categorie categorie);
        void Delete(string id);
        void Save();

        List<Categorie> SearchCategories(string searchValue);

    }
}
