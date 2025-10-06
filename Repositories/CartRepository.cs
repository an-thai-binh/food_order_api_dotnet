using FoodOrderApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderApi.Repositories
{
    public class CartRepository
    {
        private readonly AppDbContext _dbContext;

        public CartRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Cart>> GetAll()
        {
            return await _dbContext.Carts.Include(c => c.Customer).ToListAsync();
        }

        public async Task<List<Cart>> GetById(int id)
        {
            return await _dbContext.Carts.Include(c => c.Customer).Where(c => c.CartId == id).ToListAsync();
        }

        public async Task<List<Cart>> GetByCustomerId(int id)
        {
            return await _dbContext.Carts.Include(c => c.Customer).Where(c => c.CustomerId == id).ToListAsync();
        }

        public async Task<Cart> Save(Cart cart)
        {
            await _dbContext.AddAsync(cart);
            await _dbContext.SaveChangesAsync();
            return cart;
        }

        public async Task Delete(int id)
        {
            Cart? existedCart = await _dbContext.Carts.FirstOrDefaultAsync(c => c.CartId == id);
            if(existedCart != null)
            {
                _dbContext.Carts.Remove(existedCart);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
