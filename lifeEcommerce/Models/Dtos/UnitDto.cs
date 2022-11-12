using System.ComponentModel.DataAnnotations;

namespace lifeEcommerce.Models.Dtos
{
    public class UnitDto
    {
        public int Id { get; set; }

        [Display(Name = "Cover Type")]
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
