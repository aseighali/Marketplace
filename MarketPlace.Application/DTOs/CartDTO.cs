using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Application.DTOs
{

    public class CartDTO
    {
        public List<CartItem> Items { get; set; } = new();
    }
}
