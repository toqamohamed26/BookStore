using Final_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Final_Project.Reposatiory
{
    public class BookReposatiory : IBookReposatiory
    {
        BookStoreContext bookStoreContext;
        public BookReposatiory(BookStoreContext _bookStoreContext)
        {
            bookStoreContext = _bookStoreContext;
        }

        public void Delete(string id)
        {
            Book oldbook = GetBook(id);
            bookStoreContext.Books.Remove(oldbook);
            Save();
        }

        public Book GetBook(string id)
        {
            Book book = bookStoreContext.Books.Include(n=>n.categorie).Include(n=>n.author).FirstOrDefault(a => a.Id == id);
            return book;
        }
        public void Searchh(string Name)
        {
            var AuthorName = bookStoreContext.Authors.Where(x => x.Name.ToLower().Contains(Name.ToLower())).ToList();
        }
        public void Search(string Name)
        {
            var Author = bookStoreContext.Authors.ToList();
        }
        public List<Book> GetBooks()
        {
            return bookStoreContext.Books.Include(n=>n.author).ToList();
        }

        public void Insert(Book book)
        {
            bookStoreContext.Books.Add(book);
            bookStoreContext.SaveChanges();
        }

        public void Save()
        {
            bookStoreContext.SaveChanges();
        }

        public List<Book> Info(string userId)
        {
            List<Book> book = bookStoreContext.Books.Where(a => a.Author_Id == userId).Include(n=>n.categorie).ToList();
            return book;
        }
        public void Update(string id, Book book)
        {
            Book oldbook = GetBook(id);
            oldbook.Name = book.Name;
            oldbook.Title = book.Title;
            oldbook.Description = book.Description;
            oldbook.Photo = book.Photo;
            oldbook.categories_Id = book.categories_Id;
            oldbook.Salary = book.Salary;
            Save();

        }

    }
}
