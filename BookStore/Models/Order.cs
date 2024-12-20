using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [ForeignKey("customer")]
        public string CustomertId { get; set; }
        public DateOnly Date { get; set; }
        public string OrderStatus { get; set; }
        [Column(TypeName = "money")]

        public decimal TotalPrice { get; set; }
        public virtual Customer customer { get; set; }
        public virtual ICollection<OrderItems>   Items { get; set; }=new List<OrderItems>();

    }
}
