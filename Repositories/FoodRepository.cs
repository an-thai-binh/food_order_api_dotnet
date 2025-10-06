using FoodOrderApi.Dto;
using FoodOrderApi.Dto.Responses;
using FoodOrderApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderApi.Repositories
{
    public class FoodRepository
    {
        private readonly AppDbContext _dbContext;

        public FoodRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Food>> GetAll()
        {
            return await _dbContext.Foods.Include(f => f.Category).ToListAsync();
        }

        public async Task<List<Food>> GetById(int id)
        {
            return await _dbContext.Foods.Include(f => f.Category).Where(f => f.FoodId == id).ToListAsync();
        }

        public async Task<List<Food>> GetByCategoryId(int id)
        {
            return await _dbContext.Foods.Include(f => f.Category).Where(f => f.CategoryId == id).ToListAsync();
        }

        public async Task<List<Food>> SearchByName(string query)
        {
            return await _dbContext.Foods.Include(f => f.Category).Where(f => (f.FoodName ?? ""). ToLower().Contains(query)).ToListAsync();
        }

        public async Task<List<FoodWithTotalQuantity>> GetTop(int top)
        {
            return await _dbContext.Foods.Include(f => f.Category).Select(f => new FoodWithTotalQuantity
            {
                Food = f,
                TotalQuantity = _dbContext.Orderdetails.Where(od => od.FoodId == f.FoodId).Sum(od => od.Quantity)
            }).OrderByDescending(x => x.TotalQuantity)
            .Take(top)
            .ToListAsync();
        }

        public async Task<Food> Save(Food food)
        {
            await _dbContext.AddAsync(food);
            await _dbContext.SaveChangesAsync();
            return food;
        }

        public async Task Update(int id, FoodDto foodDto)
        {
            Food? food = await _dbContext.Foods.FirstOrDefaultAsync(f => f.FoodId == id);
            if(food != null)
            {
                food.CategoryId = foodDto.CategoryId;
                food.FoodName = foodDto.FoodName;
                food.Description = foodDto.Description;
                food.Price = foodDto.Price;
                food.ImgUrl = foodDto.ImgUrl;
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Food? food = await _dbContext.Foods.FirstOrDefaultAsync(f => f.FoodId == id);
            if(food != null)
            {
                _dbContext.Foods.Remove(food);
                await _dbContext.SaveChangesAsync();
            }   
        }
    }
}
