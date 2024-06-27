namespace NexusWeb.ViewModels
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public Decimal Price { get; set; }
        public int Quantity { get; set; }
        public Decimal Total => Price * Quantity;
    }
}
