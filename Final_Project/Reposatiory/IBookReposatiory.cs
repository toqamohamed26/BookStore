using Final_Project.Models;

namespace Final_Project.Reposatiory
{
    public interface IBookReposatiory
    {
        List<Book> GetBooks();
        Book GetBook(string id);
        void Insert(Book book);
        void Update( string id,Book book);
        void Delete(string id);
        void Search(string Name);
        void Searchh(string Name);
        List<Book> Info(string userId);

        void Save();
    }
}
