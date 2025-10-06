using FoodOrderApi.Dto;
using FoodOrderApi.Dto.Responses;
using FoodOrderApi.Exceptions;
using FoodOrderApi.Models;
using FoodOrderApi.Repositories;

namespace FoodOrderApi.Services
{
    public class FoodService
    {
        private readonly FoodRepository _foodRepo;

        public FoodService(FoodRepository foodRepo)
        {
            _foodRepo = foodRepo;
        }

        public async Task<List<Food>> Get(string type, int id)
        {
            List<Food> foods = new();
            switch (type)
            {
                case "all":
                    {
                        foods = await _foodRepo.GetAll();
                        break;
                    }
                case "id":
                    {
                        foods = await _foodRepo.GetById(id);
                        break;
                    }
                case "category":
                    {
                        foods = await _foodRepo.GetByCategoryId(id);
                        break;
                    }
                default:
                    {
                        throw new AppException(StatusCodes.Status400BadRequest, "Type not found");
                    }
            }
            return foods;
        }

        public async Task<List<Food>> Search(string query)
        {
            if(string.IsNullOrEmpty(query))
            {
                throw new AppException(StatusCodes.Status400BadRequest, "Search query cannot be empty");
            }
            string lowercaseQuery = query.ToLower();
            List<Food> foods = await _foodRepo.SearchByName(lowercaseQuery);
            return foods;
        }

        public async Task<List<FoodWithTotalQuantity>> GetTop(int top)
        {
            List<FoodWithTotalQuantity> foods = await _foodRepo.GetTop(top);
            return foods;
        }

        public async Task<int> Insert(FoodDto foodDto)
        {
            if(foodDto == null)
            {
                throw new AppException(StatusCodes.Status400BadRequest, "FoodDto is NULL");
            }
            Food food = new()
            {
                CategoryId = foodDto.CategoryId,
                FoodName = foodDto.FoodName,
                Description = foodDto.Description,
                Price = foodDto.Price,
                ImgUrl = foodDto.ImgUrl
            };
            food = await _foodRepo.Save(food);
            return food.FoodId;
        }

        public async Task Update(int id, FoodDto foodDto)
        {
            if (foodDto == null)
            {
                throw new AppException(StatusCodes.Status400BadRequest, "FoodDto is NULL");
            }
            List<Food> existedFoods = await _foodRepo.GetById(id);
            if(existedFoods.Count == 0)
            {
                throw new AppException(StatusCodes.Status404NotFound, $"Food with ID {id} not found");
            }
            await _foodRepo.Update(id, foodDto);
            
        }

        public async Task Delete(int id)
        {
            List<Food> existedFoods = await _foodRepo.GetById(id);
            if (existedFoods.Count == 0)
            {
                throw new AppException(StatusCodes.Status404NotFound, $"Food with ID {id} not found");
            }
            await _foodRepo.Delete(id);
        }
    }
}
