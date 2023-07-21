using Final_Project.Models;

namespace Final_Project.Reposatiory
{
    public interface IAuthorRepository
    {
        List<Author> GetAuthors();
        Author GetAuthor(string id);

        void Insert(Author author);
        void Update(string id, Author author);
        void Delete(string id);
        void Save();
    }
}
