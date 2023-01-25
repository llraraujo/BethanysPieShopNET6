using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BethanysPieShop.Models
{
    public class ShoppingCart : IShoppingCart
    {
        private readonly BethanysPieShopDbContext _bethanysPieShopDbContext;

        public string? ShoppingCartId { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; } = default!;
        public ShoppingCart(BethanysPieShopDbContext bethanysPieShopDbContext)
        {
            _bethanysPieShopDbContext = bethanysPieShopDbContext;
        }

        // Session é disponível através do sistema de Injeção de Dependência (Container)
        public static ShoppingCart GetCart(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.Session;

            BethanysPieShopDbContext context = services.GetRequiredService<BethanysPieShopDbContext>() 
                        ?? throw new Exception("Error initializing");

            string cartId = session?.GetString("CartId") ?? Guid.NewGuid().ToString();

            session?.SetString("CartId", cartId);

            return new ShoppingCart(context){ ShoppingCartId = cartId };
        }

        public void AddToCart(Pie pie)
        {
            var shoppingCartItem = _bethanysPieShopDbContext.ShoppingCartItems
                                    .SingleOrDefault(s => s.Pie.PieId == pie.PieId && s.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    Pie = pie,
                    Amount = 1
                };

                _bethanysPieShopDbContext.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }

            _bethanysPieShopDbContext.SaveChanges();
        }

        public void ClearCart()
        {
            var cartItems = _bethanysPieShopDbContext
                            .ShoppingCartItems
                            .Where(c => c.ShoppingCartId == ShoppingCartId);

            _bethanysPieShopDbContext.ShoppingCartItems.RemoveRange(cartItems);

            _bethanysPieShopDbContext.SaveChanges();
        }

        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems ??= _bethanysPieShopDbContext
                        .ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                                          .Include(s => s.Pie)
                                          .ToList(); ;
        }

        public decimal GetShoppingCartTotal()
        {
            decimal total = _bethanysPieShopDbContext.ShoppingCartItems
                            .Where(c => c.ShoppingCartId == ShoppingCartId)
                            .Select(i => i.Pie.Price * i.Amount).Sum();

            return total;
                            
        }

        public int RemoveFromCart(Pie pie)
        {
            var shoppingCartItem = _bethanysPieShopDbContext.ShoppingCartItems
                                    .SingleOrDefault(s => s.Pie.PieId == pie.PieId && s.ShoppingCartId == ShoppingCartId);

            int localAmount = 0;

            if(shoppingCartItem != null)
            {
                if(shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                    localAmount = shoppingCartItem.Amount;
                }
                else
                {
                    _bethanysPieShopDbContext.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }

            _bethanysPieShopDbContext.SaveChanges();

            return localAmount;
        }
    }
}
