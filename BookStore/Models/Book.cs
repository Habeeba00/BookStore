using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        [Column(TypeName ="money")]
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateOnly PublishedDate { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }
        public string PhotoPath { get; set; }

        [Required]
        [ForeignKey("author")]

        public int AuthorId { get; set; }
        public virtual Author author { get; set; }

        [ForeignKey("category")]

        public int CategoryId { get; set; }
        public virtual Category category { get; set; } 







    }
}
