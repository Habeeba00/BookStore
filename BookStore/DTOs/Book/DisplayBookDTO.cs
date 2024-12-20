using BookStore.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookStore.DTOs.Book
{
    public class DisplayBookDTO
    {
        public int BookId { get; set; }

        public string Title { get; set; }
      
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateOnly PublishedDate { get; set; }

        public int AuthorId { get; set; }

        public int CategoryId { get; set; }
    }
}
