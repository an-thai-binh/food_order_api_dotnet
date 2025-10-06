using FoodOrderApi.Dto;
using FoodOrderApi.Models;
using FoodOrderApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodOrderController : ControllerBase
    {
        private readonly FoodOrderService _orderService;

        public FoodOrderController(FoodOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Lấy danh sách đơn hàng
        /// </summary>
        /// <param name="type">all: lấy toàn bộ | id: lấy theo id | customer: lấy theo id khách hàng</param>
        /// <param name="id">id</param>
        /// <returns>Task<IActionResult></returns>
        [HttpGet("{type}/{id}")]
        public async Task<IActionResult> Get(string type, int id)
        {
            List<Foodorder> orders = await _orderService.Get(type, id);
            return Ok(orders);
        }

        /// <summary>
        /// Thêm đơn hàng
        /// </summary>
        /// <param name="foodOrderDto">Thông tin đơn hàng</param>
        /// <returns>Task<IActionResult></returns>
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] FoodOrderDto foodOrderDto)
        {
            int orderId = await _orderService.Insert(foodOrderDto);
            return Ok(orderId);
        }

        /// <summary>
        /// Cập nhật đơn hàng
        /// </summary>
        /// <param name="id">id đơn hàng</param>
        /// <param name="foodOrderDto">Thông tin cập nhật</param>
        /// <returns>Task<IActionResult></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] FoodOrderDto foodOrderDto)
        {
            await _orderService.Update(id, foodOrderDto);
            return Ok();
        }

        /// <summary>
        /// Xoá đơn hàng
        /// </summary>
        /// <param name="id">ID đơn hàng</param>
        /// <returns>Task<IActionResult></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderService.Delete(id);
            return Ok();
        }
    }
}
