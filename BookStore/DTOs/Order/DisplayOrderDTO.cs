using BookStore.DTOs.OrderItems;

namespace BookStore.DTOs.Order
{
    public class DisplayOrderDTO
    {
        public int OrderId { get; set; }
        public string CustomertId { get; set; }
        public DateOnly Date { get; set; }
        public string OrderStatus { get; set; }

        public decimal TotalPrice { get; set; }
        public List<DisplayOrderItemsDTO> items { get; set; } = new List<DisplayOrderItemsDTO>();

    }
}
