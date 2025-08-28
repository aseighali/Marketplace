using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Domain.Entities
{
    public class Product
    {
        public Product(string name, decimal price, Guid categoryId, string sellerId, string description = "")
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name is required.", nameof(name));

            if (price <= 0)
                throw new ArgumentException("Price must be greater than zero.", nameof(price));

            if (categoryId == Guid.Empty)
                throw new ArgumentException("Category is required.", nameof(categoryId));

            if (string.IsNullOrWhiteSpace(sellerId))
                throw new ArgumentException("SellerId is required.", nameof(sellerId));


            Id = Guid.NewGuid();
            Name = name;
            Price = price;
            CategoryId = categoryId;
            Description = description;
            SellerId = sellerId;
            IsArchived = false;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public Guid CategoryId { get; set; }
        public ProductCategory Category { get; set; }

        public string SellerId { get; set; }
        public bool IsArchived { get; set; }

        public void Archive()
        {
            IsArchived = true;
        }

        public void Update(string name, decimal price, Guid categoryId, string? description = null) 
        {
            if(string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required", nameof(name));
            if(price <= 0) throw new ArgumentException("Price needs to be higher than zero", nameof(price));
            if(categoryId == Guid.Empty) throw new ArgumentException("Category is required", nameof(categoryId));

            Name = name;
            Price = price;
            CategoryId = categoryId;
            if(description != null) Description = description;
        }
    }
}
