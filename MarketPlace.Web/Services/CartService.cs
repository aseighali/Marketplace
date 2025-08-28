using MarketPlace.Application.DTOs;
using MarketPlace.Web.Extensions;

namespace MarketPlace.Web.Services
{
    public class CartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string CartKey = "Cart";

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ISession Session => _httpContextAccessor.HttpContext!.Session;

        public Cart GetCart()
        {
            return Session.GetObject<Cart>(CartKey) ?? new Cart();
        }

        public void AddToCart(CartItem item)
        {
            var cart = GetCart();
            var existingItem = cart.Items.FirstOrDefault(c => c.ProductId == item.ProductId);
            if (existingItem == null)
            {
                cart.Items.Add(item);
                Session.SetObject(CartKey, cart);
            }
            
        }

        public void RemoveFromCart(Guid productId)
        {
            var cart = GetCart();
            cart.Items.RemoveAll(x => x.ProductId == productId);
            Session.SetObject(CartKey, cart);
        }

        public void ClearCart()
        {
            Session.Remove(CartKey);
        }
    }
}
