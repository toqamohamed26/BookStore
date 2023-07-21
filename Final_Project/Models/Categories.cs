using System.ComponentModel.DataAnnotations;

namespace Final_Project.Models
{
    public class Categorie
    {
        private string GenerateUniqueId()
        {
            Guid guid = Guid.NewGuid();
            return guid.ToString();
        }
        public Categorie()
        {
            Id = GenerateUniqueId();
        }
        [Key]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual List<Book> book { get; set; }
    }
}
