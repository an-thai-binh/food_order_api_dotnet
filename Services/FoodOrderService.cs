using FoodOrderApi.Dto;
using FoodOrderApi.Exceptions;
using FoodOrderApi.Models;
using FoodOrderApi.Repositories;

namespace FoodOrderApi.Services
{
    public class FoodOrderService
    {
        private readonly FoodOrderRepository _orderRepo;

        public FoodOrderService(FoodOrderRepository orderRepo)
        {
            _orderRepo = orderRepo;
        }

        public async Task<List<Foodorder>> Get(string type, int id)
        {
            List<Foodorder> orders = new();
            switch(type)
            {
                case "all":
                    {
                        orders = await _orderRepo.GetAll();
                        break;
                    }
                case "id":
                    {
                        orders = await _orderRepo.GetById(id);
                        break;
                    }
                case "customer":
                    {
                        orders = await _orderRepo.GetByCustomerId(id);
                        break;
                    }
                default:
                    {
                        throw new AppException(StatusCodes.Status400BadRequest, "Type not found");
                    }
            }
            return orders;
        }

        public async Task<int> Insert(FoodOrderDto foodOrderDto)
        {
            if(foodOrderDto == null)
            {
                throw new AppException(StatusCodes.Status400BadRequest, "Order is NULL");
            }
            Foodorder order = new()
            {
                CustomerId = foodOrderDto.CustomerId,
                OrderName = foodOrderDto.OrderName,
                OrderEmail = foodOrderDto.OrderEmail,
                OrderPhoneNumber = foodOrderDto.OrderPhoneNumber,
                OrderAddress = foodOrderDto.OrderAddress,
                OrderTime = foodOrderDto.OrderTime,
                TotalPrice = foodOrderDto.TotalPrice,
                Status = foodOrderDto.Status
            };
            order = await _orderRepo.Save(order);
            return order.OrderId;
        }

        public async Task Update(int id, FoodOrderDto foodOrderDto)
        {
            Foodorder? existedOrder = await _orderRepo.ShowById(id) ?? throw new AppException(StatusCodes.Status404NotFound, "Not found");
            await _orderRepo.Update(id, foodOrderDto);
        }

        public async Task Delete(int id)
        {
            Foodorder? existedOrder = await _orderRepo.ShowById(id) ?? throw new AppException(StatusCodes.Status404NotFound, "Not found");
            await _orderRepo.Delete(id);
        }
    }
}
