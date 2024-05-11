using System.ComponentModel.DataAnnotations;

namespace lr5.Models
{
    public class Category
    {
        [Key]
        public int category_id { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [StringLength(30, ErrorMessage = "Product brand must be between 2 and 30 characters", MinimumLength = 2)]
        public string category_name { get; set; }

        [StringLength(30, ErrorMessage = "Product brand must be between 2 and 255 characters", MinimumLength = 2)]
        public string? category_desc { get; set;}
    }
}
