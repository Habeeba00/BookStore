using System.ComponentModel.DataAnnotations;

namespace BookStore.DTOs.Password
{
    public class ChangePasswordDTO
    {
        [Required(ErrorMessage = "User ID is required.")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Old password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Old password must be at least 6 characters.")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "New password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "New password must be at least 6 characters.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm password is required.")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
