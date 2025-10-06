using FoodOrderApi.Dto;
using FoodOrderApi.Exceptions;
using FoodOrderApi.Models;
using FoodOrderApi.Repositories;

namespace FoodOrderApi.Services
{
    public class CartItemService
    {
        private readonly CartItemRepository _cartItemRepo;

        public CartItemService(CartItemRepository cartItemRepo)
        {
            _cartItemRepo = cartItemRepo;
        }

        public async Task<List<Cartitem>> Get(string type, int id)
        {
            List<Cartitem> items = new();
            switch(type)
            {
                case "all":
                    {
                        items = await _cartItemRepo.GetAll();
                        break;
                    }
                case "id":
                    {
                        items = await _cartItemRepo.GetByFoodId(id);
                        break;
                    }
                case "cart":
                    {
                        items = await _cartItemRepo.GetByCartId(id);
                        break;
                    }
                case "customer":
                    {
                        items = await _cartItemRepo.GetByCustomerId(id);
                        break;
                    }
                default:
                    {
                        throw new AppException(StatusCodes.Status400BadRequest, "Type not found");
                    }
            }
            return items;
        }

        public async Task<int> Insert(CartItemDto cartItemDto)
        {
            if(cartItemDto == null)
            {
                throw new AppException(StatusCodes.Status400BadRequest, "CartItem is NULL");
            }
            Cartitem? existedItem = await _cartItemRepo.ShowByCartIdAndFoodId(cartItemDto.CartId, cartItemDto.FoodId);
            if(existedItem != null)
            {
                await _cartItemRepo.UpdateQuantity(existedItem.ItemId, existedItem.Quantity + cartItemDto.Quantity);
                return existedItem.ItemId;
            }
            Cartitem cartItem = new()
            {
                CartId = cartItemDto.CartId,
                FoodId = cartItemDto.FoodId,
                Quantity = cartItemDto.Quantity
            };
            cartItem = await _cartItemRepo.Save(cartItem);
            return cartItem.ItemId;
        }

        public async Task UpdateQuantity(int id, int quantity)
        {
            Cartitem? existedItem = await _cartItemRepo.ShowById(id) ?? throw new AppException(StatusCodes.Status404NotFound, $"CartItem with ID {id} not found");
            await _cartItemRepo.UpdateQuantity(id, quantity);
        }

        public async Task DeleteById(int id)
        {
            Cartitem? existedItem = await _cartItemRepo.ShowById(id) ?? throw new AppException(StatusCodes.Status404NotFound, $"CartItem with ID {id} not found");
            await _cartItemRepo.Delete(id);
        }

        public async Task DeleteByType(string type, int id)
        {
            switch(type) {
                case "customer":
                    {
                        await _cartItemRepo.DeleteByCustomerId(id);
                        break;
                    }
                case "cart":
                    {
                        await _cartItemRepo.DeleteByCartId(id);
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
