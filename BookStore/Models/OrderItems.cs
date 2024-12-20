using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    public class OrderItems
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int OrderItemId { get; set; }

        [ForeignKey("order")]
        public int OrderId { get; set; }
        public virtual Order order { get; set; }

        [ForeignKey("book")]

        public int BookId { get; set; }
        public virtual Book book { get; set; }
        [Column(TypeName = "money")]

        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }



    }
}
