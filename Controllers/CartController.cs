using FoodOrderApi.Dto;
using FoodOrderApi.Models;
using FoodOrderApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        /// <summary>
        /// Lấy danh sách giỏ hàng
        /// </summary>
        /// <param name="type">all: lấy toàn bộ | id: lấy theo id giỏ hàng | customer: lấy theo id khách hàng</param>
        /// <param name="id">id</param>
        /// <returns>IActionResult</returns>
        [HttpGet("{type}/{id}")]
        public async Task<IActionResult> Get(string type, int id)
        {
            List<Cart> carts = await _cartService.Get(type, id);
            return Ok(carts);
        }

        /// <summary>
        /// Thêm giỏ hàng
        /// </summary>
        /// <param name="cartDto">Thông tin giỏ hàng</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] CartDto cartDto)
        {
            int cartId = await _cartService.Insert(cartDto);
            return Ok(cartId);
        }

        /// <summary>
        /// Xoá giỏ hàng
        /// </summary>
        /// <param name="id">id giỏ hàng</param>
        /// <returns>IActionResult</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _cartService.Delete(id);
            return Ok();
        }
    }

}
