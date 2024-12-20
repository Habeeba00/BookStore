using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int? Age { get; set; }
        public string? Bio { get; set; }
        public int? NumberOfBooks { get; set; }
        public virtual ICollection<Book> Books { get; set; }=new List<Book>();




    }
}
