using FoodOrderApi.Dto;
using FoodOrderApi.Exceptions;
using FoodOrderApi.Models;
using FoodOrderApi.Repositories;

namespace FoodOrderApi.Services
{
    public class OrderDetailService
    {
        private readonly OrderDetailRepository _detailRepo;

        public OrderDetailService(OrderDetailRepository detailRepo)
        {
            _detailRepo = detailRepo;
        }

        public async Task<List<Orderdetail>> Get(string type, int id)
        {
            List<Orderdetail> details = new();
            switch(type)
            {
                case "all":
                    {
                        details = await _detailRepo.GetAll();
                        break;
                    }
                case "id":
                    {
                        details = await _detailRepo.GetById(id);
                        break;
                    }
                case "order":
                    {
                        details = await _detailRepo.GetByOrderId(id);
                        break;
                    }
                case "customer":
                    {
                        details = await _detailRepo.GetByCustomerId(id);
                        break;
                    }
                default:
                    {
                        throw new AppException(StatusCodes.Status400BadRequest, "Type not found");
                    }
            }
            return details;
        }

        public async Task<int> Insert(OrderDetailDto detailDto)
        {
            if (detailDto == null)
            {
                throw new AppException(StatusCodes.Status400BadRequest, "Detail is NULL");
            }
            Orderdetail detail = new()
            {
                OrderId = detailDto.OrderId,
                FoodId = detailDto.FoodId,
                Quantity = detailDto.Quantity
            };
            detail = await _detailRepo.Save(detail);
            return detail.DetailId;
        }

        public async Task Update(int id, OrderDetailDto detailDto)
        {
            if (detailDto == null)
            {
                throw new AppException(StatusCodes.Status400BadRequest, "Detail is NULL");
            }
            Orderdetail existedDetail = await _detailRepo.ShowById(id) ?? throw new AppException(StatusCodes.Status404NotFound, $"Detail with ID {id} not found");
            await _detailRepo.Update(id, detailDto);
        }

        public async Task Delete(string type, int id)
        {
            switch (type)
            {
                case "id":
                    {
                        await _detailRepo.DeleteById(id);
                        break;
                    }
                case "customer":
                    {
                        await _detailRepo.DeleteByCustomerId(id);
                        break;
                    }
                default:
                    {
                        throw new AppException(StatusCodes.Status400BadRequest, "Type not found");
                    }
            }
        }
    }
}
