using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Random.App.ProductManagement.Domain.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int OriginalApiId {  get; set; }

        [Required]
        [StringLength(200)]  
        public string Name { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double Price { get; set; }

        [Required]
        [StringLength(100)]
        public string Category { get; set; }

        [StringLength(1000)]  
        public string Description { get; set; }

        public string Image { get; set; }
    }
}
