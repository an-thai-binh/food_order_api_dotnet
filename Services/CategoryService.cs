using FoodOrderApi.Dto;
using FoodOrderApi.Exceptions;
using FoodOrderApi.Models;
using FoodOrderApi.Repositories;

namespace FoodOrderApi.Services
{
    public class CategoryService
    {
        private readonly CategoryRepository _categoryRepo;

        public CategoryService(CategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<List<Category>> Get(string type, int id)
        {
            List<Category> categories = new();
            switch (type) {
                case "all":
                    {
                        categories = await _categoryRepo.GetAll();
                        break;
                    }
                case "id":
                    {
                        categories = await _categoryRepo.GetId(id);
                        break;
                    }
                default:
                    {
                        throw new AppException(StatusCodes.Status400BadRequest, "Type not found");
                    }
            }
            return categories;
        }

        public async Task<int> Insert(CategoryDto categoryDto)
        {
            if(categoryDto == null)
            {
                throw new AppException(StatusCodes.Status400BadRequest, "Category is NULL");
            }
            Category category = new()
            {
                CategoryName = categoryDto.CategoryName,
                CategoryImgUrl = categoryDto.CategoryImgUrl
            };
            category = await _categoryRepo.Save(category);
            return category.CategoryId;
        }

        public async Task Update(int id, CategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                throw new AppException(StatusCodes.Status400BadRequest, "Category is NULL");
            }
            List<Category> existedCategories = await _categoryRepo.GetId(id);
            if(existedCategories.Count == 0)
            {
                throw new AppException(StatusCodes.Status404NotFound, $"Category with ID {id} not found");
            }
            await _categoryRepo.Update(id, categoryDto);
        }

        public async Task Delete(int id)
        {
            List<Category> existedCategories = await _categoryRepo.GetId(id);
            if (existedCategories.Count == 0)
            {
                throw new AppException(StatusCodes.Status404NotFound, $"Category with ID {id} not found");
            }
            await _categoryRepo.Delete(id);
        }
    }
}
