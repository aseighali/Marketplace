using MarketPlace.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Application.DTOs
{
    public class ProductDto
    {
        // Parameterless constructor needed for model binding
        public ProductDto() { }
        public ProductDto(Guid id, string name, decimal price, Guid categoryId, string categoryName)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Price = price;
            Description = string.Empty;
            CategoryId = categoryId;
            CategoryName = categoryName ?? throw new ArgumentNullException(nameof(categoryName));
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? SellerId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Category is required")]
        public Guid CategoryId { get; set; }
        public string CategoryName { get; private set; } = string.Empty;

        public static ProductDto FromDomain(Product product)
        {
            return new ProductDto(
                product.Id,
                product.Name,
                product.Price,
                product.CategoryId,
                product.Category.Name
            )
            {
                SellerId = product.SellerId,
                Description = product.Description

            };

        }
    }

}
