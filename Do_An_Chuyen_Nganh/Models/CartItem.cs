namespace Do_An_Chuyen_Nganh.Models
{
    public class CartItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
