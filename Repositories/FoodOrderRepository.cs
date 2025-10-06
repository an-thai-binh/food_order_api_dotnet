using FoodOrderApi.Dto;
using FoodOrderApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderApi.Repositories
{
    public class FoodOrderRepository
    {
        private readonly AppDbContext _dbContext;

        public FoodOrderRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Foodorder>> GetAll()
        {
            return await _dbContext.Foodorders.Include(o => o.Customer).ToListAsync();
        }

        public async Task<List<Foodorder>> GetById(int id)
        {
            return await _dbContext.Foodorders.Include(o => o.Customer).Where(o => o.OrderId == id).ToListAsync();
        }

        public async Task<List<Foodorder>> GetByCustomerId(int id)
        {
            return await _dbContext.Foodorders.Include(o => o.Customer).Where(o => o.CustomerId == id).ToListAsync();
        }

        public async Task<Foodorder> Save(Foodorder order)
        {
            await _dbContext.AddAsync(order);
            await _dbContext.SaveChangesAsync();
            return order;
        }

        public async Task<Foodorder?> ShowById(int id)
        {
            return await _dbContext.Foodorders.FirstOrDefaultAsync(o => o.OrderId == id);
        }

        public async Task Update(int id, FoodOrderDto foodOrderDto)
        {
            Foodorder? existedOrder = await _dbContext.Foodorders.FirstOrDefaultAsync(o => o.OrderId == id);
            if(existedOrder != null)
            {
                existedOrder.CustomerId = foodOrderDto.CustomerId;
                existedOrder.OrderName = foodOrderDto.OrderName;
                existedOrder.OrderEmail = foodOrderDto.OrderEmail;
                existedOrder.OrderPhoneNumber = foodOrderDto.OrderPhoneNumber;
                existedOrder.OrderAddress = foodOrderDto.OrderAddress;
                existedOrder.OrderTime = foodOrderDto.OrderTime;
                existedOrder.TotalPrice = foodOrderDto.TotalPrice;
                existedOrder.Status = foodOrderDto.Status;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            Foodorder? existedOrder = await _dbContext.Foodorders.FirstOrDefaultAsync(o => o.OrderId == id);
            if (existedOrder != null)
            {
                _dbContext.Foodorders.Remove(existedOrder);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
