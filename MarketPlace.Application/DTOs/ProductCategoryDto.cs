using MarketPlace.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Application.DTOs
{
    public class ProductCategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        // Parameterless constructor needed for model binding
        public ProductCategoryDto() { }

        public ProductCategoryDto(Guid id, string name)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public static ProductCategoryDto FromDomain(ProductCategory category)
        {
            return new ProductCategoryDto(category.Id, category.Name);
        }
    }
}
