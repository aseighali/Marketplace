namespace MarketPlace.Application.DTOs
{
    public class CartItem
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}