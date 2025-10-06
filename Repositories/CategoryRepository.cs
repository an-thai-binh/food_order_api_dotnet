using FoodOrderApi.Dto;
using FoodOrderApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderApi.Repositories
{
    public class CategoryRepository
    {
        private readonly AppDbContext _dbContext;

        public CategoryRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Category>> GetAll()
        {
            return await _dbContext.Categories.ToListAsync();
        }
        
        public async Task<List<Category>> GetId(int id)
        {
            return await _dbContext.Categories.Where(c => c.CategoryId == id).ToListAsync();
        }

        public async Task<Category> Save(Category category)
        {
            await _dbContext.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            return category;
        }

        public async Task Update(int id, CategoryDto categoryDto)
        {
            Category? category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
            if(category != null)
            {
                category.CategoryName = categoryDto.CategoryName;
                category.CategoryImgUrl = categoryDto.CategoryImgUrl;
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Category? category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
            if (category != null)
            {
                _dbContext.Categories.Remove(category);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
