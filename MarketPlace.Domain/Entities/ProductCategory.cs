using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Domain.Entities
{
    public class ProductCategory
    {
        public ProductCategory(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("Name is required.", nameof(name));
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsArchived { get; set; }

        public void Archive()
        {
            IsArchived = true;
        }

        public void Update(string name)
        {
            if(string.IsNullOrEmpty(name)) throw new ArgumentNullException("Name is required.", nameof(name));

            Name = name;
        }
    }
}
