using Do_An_Chuyen_Nganh.Infrastructure;
using Do_An_Chuyen_Nganh.Models;

namespace Do_An_Chuyen_Nganh.Service
{
    public class CartManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public const string CartSessionKey = "Cart";

        public List<CartItem> GetCartItems()
        {
            var cart = _httpContextAccessor.HttpContext.Session.GetObject<List<CartItem>>(CartSessionKey);

            if (cart == null)
            {
                cart = new List<CartItem>();
                _httpContextAccessor.HttpContext.Session.SetObject(CartSessionKey, cart);
            }

            return cart;
        }

        public void AddToCart(CartItem item)
        {
            var cart = GetCartItems();
            var existingItem = cart.FirstOrDefault(c => c.ProductId == item.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                cart.Add(item);
            }
            
            UpdateCart(cart);
        }

        public void UpdateCart(IEnumerable<CartItem> cart)
        {
            _httpContextAccessor.HttpContext.Session.SetObject(CartSessionKey, cart.ToList());
        }

        public void RemoveFromCart(Guid cartItemId)
        {
            var cart = GetCartItems();
            var existingItem = cart.FirstOrDefault(c => c.Id == cartItemId);

            if (existingItem != null)
            {
                cart.Remove(existingItem);
            }
            UpdateCart(cart);
        }

        public decimal GetCartTotal(IEnumerable<CartItem> cart)
        {
            decimal total = 0;

            foreach (var item in cart)
            {
                total += item.Price * item.Quantity;
            }

            return total;
        }

        public decimal GetTotal()
        {
            var cart = GetCartItems();
            decimal total = 0;

            foreach (var item in cart)
            {
                total += item.Price * item.Quantity;
            }

            return total;
        }

        public void ClearCart()
        {
            var cart = GetCartItems();
            cart.Clear();
            UpdateCart(cart);
        }
    }
}
