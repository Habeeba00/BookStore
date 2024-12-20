using Microsoft.AspNetCore.Identity;

namespace BookStore.Models
{
    public class Customer:IdentityUser
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public virtual ICollection<Order> orders { get; set; } = new List<Order>();


    }
}
