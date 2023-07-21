using Final_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Final_Project.Reposatiory
{
    public class AuthorRepository : IAuthorRepository
    {
        BookStoreContext bookStoreContext;

        public ClaimsPrincipal User { get; private set; }
        public AuthorRepository(
            BookStoreContext _bookStoreContext


            )
        {
            bookStoreContext = _bookStoreContext;

        }
        private string GenerateUniqueId()
        {
            Guid guid = Guid.NewGuid();
            return guid.ToString();
        }


        public Author GetAuthor(string id)
        {
            Author author = bookStoreContext.Authors.FirstOrDefault(a => a.Id == id);
            return author;
        }

        public List<Author> GetAuthors()
        {
            return bookStoreContext.Authors.Include(e => e.Book).ToList();
        }


        public void Insert(Author author)
        {
            author.Id = GenerateUniqueId(); // Generate unique ID
            bookStoreContext.Authors.Add(author);
            bookStoreContext.SaveChanges();
        }

        public void Save()
        {
            bookStoreContext.SaveChanges();
        }

        public void Update(string id, Author author)
        {
            //get old
            Author oldAuthor = GetAuthor(id);
            oldAuthor.Name = author.Name;
            oldAuthor.Email = author.Email;
            oldAuthor.Book = author.Book;
            oldAuthor.Password = author.Password;
            oldAuthor.Photo = author.Photo;


        }
        public void Delete(string id)
        {
            Author oldAuthor = GetAuthor(id);
            bookStoreContext.Authors.Remove(oldAuthor);
            bookStoreContext.SaveChanges();
        }
    }

}
