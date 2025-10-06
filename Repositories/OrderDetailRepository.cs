using FoodOrderApi.Dto;
using FoodOrderApi.Exceptions;
using FoodOrderApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderApi.Repositories
{
    public class OrderDetailRepository
    {
        private readonly AppDbContext _dbContext;

        public OrderDetailRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Orderdetail>> GetAll()
        {
            return await _dbContext.Orderdetails.Include(d => d.Food).ToListAsync();
        }
        public async Task<List<Orderdetail>> GetById(int id)
        {
            return await _dbContext.Orderdetails.Include(d => d.Food).Where(d => d.DetailId == id).ToListAsync();
        }

        public async Task<List<Orderdetail>> GetByOrderId(int id)
        {
            return await _dbContext.Orderdetails.Include(d => d.Food).Where(d => d.OrderId == id).ToListAsync();
        }

        public async Task<List<Orderdetail>> GetByCustomerId(int id)
        {
            return await _dbContext.Orderdetails.Include(d => d.Food).Where(d => d.Order.CustomerId == id).ToListAsync();
        }

        public async Task<Orderdetail?> ShowById(int id)
        {
            return await _dbContext.Orderdetails.FirstOrDefaultAsync(d => d.DetailId == id);
        }

        public async Task<Orderdetail> Save(Orderdetail detail)
        {
            
            await _dbContext.AddAsync(detail);
            await _dbContext.SaveChangesAsync();
            return detail;
        }

        public async Task Update(int id, OrderDetailDto detailDto)
        {
            Orderdetail? existedDetail = await _dbContext.Orderdetails.FirstOrDefaultAsync(d => d.DetailId == id);
            if(existedDetail != null)
            {
                existedDetail.OrderId = detailDto.OrderId;
                existedDetail.FoodId = detailDto.FoodId;
                existedDetail.Quantity = detailDto.Quantity;
                await _dbContext.SaveChangesAsync();
            }
       }

        public async Task DeleteById(int id)
        {
            Orderdetail? existedDetail = await _dbContext.Orderdetails.FirstOrDefaultAsync(d => d.DetailId == id);
            if (existedDetail != null)
            {
                _dbContext.Orderdetails.Remove(existedDetail);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteByCustomerId(int id)
        {
            List<Orderdetail> existedDetails = await _dbContext.Orderdetails.Where(d => d.Order.CustomerId == id).ToListAsync();
            if (existedDetails.Count > 0)
            {
                _dbContext.Orderdetails.RemoveRange(existedDetails);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
