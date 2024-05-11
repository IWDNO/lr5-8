using System.ComponentModel.DataAnnotations;

namespace lr5.Models
{
    public class Product
    {
        [Key]
        [Required]
        public int Product_id { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(50, ErrorMessage = "Product name must be between 3 and 50 characters", MinimumLength = 3)]
        public string ProductName { get; set; } = "";

        [Required(ErrorMessage = "Product brand is required")]
        [StringLength(30, ErrorMessage = "Product brand must be between 2 and 30 characters", MinimumLength = 2)]
        public string ProductBrand { get; set; } = "";

        [Required(ErrorMessage = "Product price is required")]
        [Range(0.01, 100000, ErrorMessage = "Product price must be between 0.01 and 100000")]
        public float ProductPrice { get; set; }

        [Required(ErrorMessage = "Processor frequency is required")]
        [Range(1.0, 5.0, ErrorMessage = "Processor frequency must be between 1.0 and 5.0")]
        public float ProcessorFrequency { get; set; }

        [Required(ErrorMessage = "RAM size is required")]
        [Range(2.0, 128.0, ErrorMessage = "RAM size must be between 2.0 and 128.0")]
        public float RamSize { get; set; }

        [Required(ErrorMessage = "Storage size is required")]
        [Range(64.0, 8192.0, ErrorMessage = "Storage size must be between 64.0 and 8192.0")]
        public float StorageSize { get; set; }

        [Required(ErrorMessage = "Categiry is required")]
        public int category_id { get; set; }
    }
}
