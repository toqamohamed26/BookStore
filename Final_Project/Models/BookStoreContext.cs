using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Final_Project.Models
{
    public class BookStoreContext : IdentityDbContext<ApplicationUser>
    {

        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Categorie> Categories { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public BookStoreContext(DbContextOptions<BookStoreContext> options)
               : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categorie>()
                .HasIndex(c => c.Name)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }

    }
}
