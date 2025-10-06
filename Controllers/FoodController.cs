using FoodOrderApi.Dto;
using FoodOrderApi.Dto.Responses;
using FoodOrderApi.Models;
using FoodOrderApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly FoodService _foodService;

        public FoodController(FoodService foodService)
        {
            _foodService = foodService;
        }

        /// <summary>
        /// Lấy (danh sách) món ăn
        /// </summary>
        /// <param name="type">all - lấy toàn bộ | id - lấy theo id | category - lấy theo id của thể loại</param>
        /// <param name="id">id</param>
        /// <returns>IActionResult</returns>
        [HttpGet("{type}/{id}")]
        public async Task<IActionResult> Get(string type, int id)
        {
            List<Food> foods = await _foodService.Get(type, id);
            return Ok(foods);
        }

        /// <summary>
        /// Tìm kiếm món ăn theo tên
        /// </summary>
        /// <param name="query">từ khoá</param>
        /// <returns>IActionResult</returns>
        [HttpGet("search/{query}")]
        public async Task<IActionResult> Search(string query)
        {
            List<Food> foods = await _foodService.Search(query);
            return Ok(foods);
        }

        /// <summary>
        /// Tìm kiếm món ăn theo tên
        /// </summary>
        /// <param name="query">từ khoá</param>
        /// <returns>IActionResult</returns>
        [HttpGet("top/{top}")]
        public async Task<IActionResult> Search(int top)
        {
            List<FoodWithTotalQuantity> foods = await _foodService.GetTop(top);
            return Ok(foods);
        }

        /// <summary>
        /// Thêm món ăn
        /// </summary>
        /// <param name="foodDto">Thông tin món ăn</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] FoodDto foodDto)
        {
            int foodId = await _foodService.Insert(foodDto);
            return Ok(foodId);
        }

        /// <summary>
        /// Cập nhật thông tin món ăn
        /// </summary>
        /// <param name="id">ID món ăn</param>
        /// <param name="foodDto">Thông tin cập nhật</param>
        /// <returns>IActionResult</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] FoodDto foodDto)
        {
            await _foodService.Update(id, foodDto);
            return Ok();
        }

        /// <summary>
        /// Xoá món ăn
        /// </summary>
        /// <param name="id">ID món ăn</param>
        /// <returns>IActionResult</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _foodService.Delete(id);
            return Ok();
        }
    }
}
