using System.ComponentModel.DataAnnotations;

namespace BookStore.DTOs.Account
{
    public class LoginResponseDTO
    { 
        public string Id { get; set; }
        public string Token { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }

    }
}
