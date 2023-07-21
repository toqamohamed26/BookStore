using System.ComponentModel.DataAnnotations;

namespace Final_Project.Models
{
    public class Author
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string?Photo { get; set; }
        public virtual List<Book> Book { get; set; }
    }
}
