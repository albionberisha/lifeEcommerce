using System.ComponentModel.DataAnnotations.Schema;

namespace lifeEcommerce.Models.Entities
{
    public class OrderDetailsCreateDto
    {
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public OrderData OrderData { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Count { get; set; }

        public decimal Price { get; set; }
    }
}
