using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Project.Models
{
    public class Book
    {
        private string GenerateUniqueId()
        {
            Guid guid = Guid.NewGuid();
            return guid.ToString();
        }
        public Book()
        {
            Id = GenerateUniqueId();
        }
        [Key]
        public string Id { get; set; }
        [DefaultValue(" ")]
        public string Name { get; set; }
        [DefaultValue(" ")]
        public string Title { get; set; }
        [DefaultValue(" ")]
        public string Description { get; set; }
        public string ? Photo { get; set; }
        public double Salary { get; set; }

        [ForeignKey(nameof(author))]
        public string ?Author_Id { get; set; }
        public virtual Author ?author  { get; set; }
        [ForeignKey(nameof(categorie))]
        public string? categories_Id { get; set; }
        public virtual Categorie? categorie { get; set; }

    }
}
