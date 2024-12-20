using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.DTOs.OrderItems
{
    public class DisplayOrderItemsDTO:AddOrderItemsDTO
    {
        public int OrderItemId { get; set; }


    }
}
