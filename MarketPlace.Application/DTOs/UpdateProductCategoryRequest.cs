using System.ComponentModel.DataAnnotations;


namespace MarketPlace.Application.DTOs
{
    public class UpdateProductCategoryRequest
    {
        [Required(ErrorMessage = "Category ID is required.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [MaxLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
        public string Name { get; set; }
    }
}
