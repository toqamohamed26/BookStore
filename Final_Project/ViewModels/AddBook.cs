using Final_Project.Models;
using Microsoft.Build.Framework;
using System.ComponentModel;

namespace Final_Project.ViewModels
{
    public class AddBook
    {
        public string? Id { get; set; }
        [Required]
        public string Name { get; set; }
        [DefaultValue(" ")]
        [Required]

        public string Title { get; set; }
        [Required]

        [DefaultValue(" ")]
        public string Description { get; set; }
        public string? Photo { get; set; } = "default.png";
        [Required]
        public double Salary { get; set; }
        public string Author_Id { get; set; }
        public string categories_Id { get; set; }
        public  List<Categorie>? Cat { get; set; }
        
    }
}
