using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BookStore.DTOs.Book
{
    public class AddBookDTO
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 200 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Published Date is required.")]
        public DateOnly PublishedDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be a valid category.")]
        public int? CategoryId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "AuthorId must be a valid author.")]
        public int? AuthorId { get; set; }

        [FileExtensions(Extensions = "jpg,jpeg,png,gif", ErrorMessage = "Only image files are allowed.")]
        public IFormFile? Photo { get; set; }
    }
}
