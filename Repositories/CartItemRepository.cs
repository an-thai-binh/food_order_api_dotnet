using FoodOrderApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderApi.Repositories
{
    public class CartItemRepository
    {
        private readonly AppDbContext _dbContext;

        public CartItemRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Cartitem>> GetAll()
        {
            return await _dbContext.Cartitems.Include(c => c.Cart).Include(c => c.Food).ToListAsync();
        }
        public async Task<List<Cartitem>> GetByFoodId(int id)
        {
            return await _dbContext.Cartitems.Include(c => c.Cart).Include(c => c.Food).Where(c => c.FoodId == id).ToListAsync();
        }

        public async Task<List<Cartitem>> GetByCartId(int id)
        {
            return await _dbContext.Cartitems.Include(c => c.Cart).Include(c => c.Food).Where(c => c.CartId == id).ToListAsync();
        }

        public async Task<List<Cartitem>> GetByCustomerId(int id)
        {
            return await _dbContext.Cartitems.Include(c => c.Cart).Include(c => c.Food).Where(c => c.Cart.CustomerId == id).ToListAsync();
        }

        public async Task<Cartitem?> ShowById(int id)
        {
            return await _dbContext.Cartitems.FirstOrDefaultAsync(c => c.ItemId == id);
        }

        public async Task<Cartitem?> ShowByCartIdAndFoodId(int cartId, int foodId)
        {
            return await _dbContext.Cartitems.FirstOrDefaultAsync(c => c.CartId == cartId && c.FoodId == foodId);
        }

        public async Task UpdateQuantity(int itemId, int? newQuantity)
        {
            Cartitem? existedItem = await _dbContext.Cartitems.FirstOrDefaultAsync(c => c.ItemId == itemId);
            if(existedItem != null)
            {
                existedItem.Quantity = newQuantity;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<Cartitem> Save(Cartitem cartItem)
        {
            await _dbContext.AddAsync(cartItem);
            await _dbContext.SaveChangesAsync();
            return cartItem;
        }

        public async Task Delete(int id)
        {
            Cartitem? existedItem = await _dbContext.Cartitems.FirstOrDefaultAsync(c => c.ItemId == id);
            if(existedItem != null)
            {
                _dbContext.Cartitems.Remove(existedItem);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteByCustomerId(int id)
        {
            var cartItems = await _dbContext.Cartitems.Where(c => c.Cart.CustomerId == id).ToListAsync();
            if(cartItems.Count > 0)
            {
                _dbContext.Cartitems.RemoveRange(cartItems);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteByCartId(int id)
        {
            var cartItems = await _dbContext.Cartitems.Where(c => c.CartId == id).ToListAsync();
            if (cartItems.Count > 0)
            {
                _dbContext.Cartitems.RemoveRange(cartItems);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
