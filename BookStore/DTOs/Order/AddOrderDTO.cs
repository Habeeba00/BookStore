using System.ComponentModel.DataAnnotations;
using BookStore.DTOs.OrderItems;
using BookStore.Models;

namespace BookStore.DTOs.Order
{
    public class AddOrderDTO
    {
        [Required(ErrorMessage = "CustomerId is required.")]
        public string CustomertId { get; set; }

        [Required(ErrorMessage = "Order Status is required.")]
        [StringLength(50, MinimumLength =5,ErrorMessage = "Order Status cannot exceed 50 characters.")]
        public string OrderStatus { get; set; }

        [Required(ErrorMessage = "Order Items are required.")]
        public List<AddOrderItemsDTO> items { get; set; } = new List<AddOrderItemsDTO>();
    }
}
