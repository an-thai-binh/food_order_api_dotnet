using FoodOrderApi.Dto;
using FoodOrderApi.Models;
using FoodOrderApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly CartItemService _cartItemService;

        public CartItemController(CartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        /// <summary>
        /// Lấy danh sách chi tiết giỏ hàng
        /// </summary>
        /// <param name="type">all: lấy toàn bộ | id: lấy theo id | cart: lấy theo id giỏ hàng | customer: lấy theo id customer</param>
        /// <param name="id">id</param>
        /// <returns>IActionResult</returns>
        [HttpGet("{type}/{id}")]
        public async Task<IActionResult> Get(string type, int id)
        {
            List<Cartitem> items = await _cartItemService.Get(type, id);
            return Ok(items);
        }

        /// <summary>
        /// Thêm chi tiết giỏ hàng
        /// </summary>
        /// <param name="cartItemDto">thông tin món ăn</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] CartItemDto cartItemDto)
        {
            int itemId = await _cartItemService.Insert(cartItemDto);
            return Ok(itemId);
        }

        /// <summary>
        /// Cập nhật số lượng của món ăn
        /// </summary>
        /// <param name="id">id chi tiết</param>
        /// <param name="quantity">số lượng</param>
        /// <returns>IActionResult</returns>
        [HttpPut("{id}/{quantity}")]
        public async Task<IActionResult> UpdateQuantity(int id, int quantity)
        {
            await _cartItemService.UpdateQuantity(id, quantity);
            return Ok();
        }

        /// <summary>
        /// Xoá món ăn trong giỏ hàng
        /// </summary>
        /// <param name="id">id chi tiết</param>
        /// <returns>IActionResult</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _cartItemService.DeleteById(id);
            return Ok();
        }

        /// <summary>
        /// Xoá món ăn trong giỏ hàng bằng id đặc biệt
        /// </summary>
        /// <param name="type">customer: xoá theo id khách hàng | cart: xoá theo id giỏ hàng</param>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpDelete("{type}/{id}")]
        public async Task<IActionResult> Delete(string type, int id)
        {
            await _cartItemService.DeleteByType(type, id);
            return Ok();
        }
    }
}
