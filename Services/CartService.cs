using FoodOrderApi.Dto;
using FoodOrderApi.Exceptions;
using FoodOrderApi.Models;
using FoodOrderApi.Repositories;

namespace FoodOrderApi.Services
{
    public class CartService
    {
        private readonly CartRepository _cartRepo;

        public CartService(CartRepository cartRepo)
        {
            _cartRepo = cartRepo;
        }

        public async Task<List<Cart>> Get(string type, int id)
        {
            List<Cart> carts = new();
            switch(type)
            {
                case "all":
                    {
                        carts = await _cartRepo.GetAll();
                        break;
                    }
                case "id":
                    {
                        carts = await _cartRepo.GetById(id);
                        break;
                    }
                case "customer":
                    {
                        carts = await _cartRepo.GetByCustomerId(id);
                        break;
                    }
                default:
                    {
                        throw new AppException(StatusCodes.Status400BadRequest, "Type not found");
                    }
            }
            return carts;
        }

        public async Task<int> Insert(CartDto cartDto)
        {
            if(cartDto == null)
            {
                throw new AppException(StatusCodes.Status400BadRequest, "Cart is NULL");
            }
            Cart cart = new()
            {
                CustomerId = cartDto.CustomerId
            };
            cart = await _cartRepo.Save(cart);
            return cart.CartId;
        }

        public async Task Delete(int id)
        {
            List<Cart> existedCarts = await _cartRepo.GetById(id);
            if(existedCarts.Count == 0)
            {
                throw new AppException(StatusCodes.Status404NotFound, $"Cart with ID {id} not found.");
            }
            await _cartRepo.Delete(id);
        }
    }
}
