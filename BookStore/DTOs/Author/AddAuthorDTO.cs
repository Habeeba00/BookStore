using System.ComponentModel.DataAnnotations;

namespace BookStore.DTOs.Author
{
    public class AddAuthorDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }

        [Range(18, 120, ErrorMessage = "Age must be between 18 and 120.")]
        public int? Age { get; set; }

        [StringLength(1000,MinimumLength = 5, ErrorMessage = "Bio cannot be longer than 1000 characters.")]
        public string? Bio { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Number of books must be a positive number.")]
        public int? NumberOfBooks { get; set; }
    }
}
