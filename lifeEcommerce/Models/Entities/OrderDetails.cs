using System.ComponentModel.DataAnnotations.Schema;

namespace lifeEcommerce.Models.Entities
{
    public class OrderDetails
    {
        public int Id { get; set; }

        public string OrderId { get; set; }
        [ForeignKey("OrderId")]
        public OrderData OrderData { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Count { get; set; }

        public decimal Price { get; set; }
    }
}
