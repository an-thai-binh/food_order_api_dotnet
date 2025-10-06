using FoodOrderApi.Dto;
using FoodOrderApi.Models;
using FoodOrderApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Lấy (danh sách) thể loại
        /// </summary>
        /// <param name="type">all - lấy toàn bộ | id - lấy theo id
        /// <param name="id">id</param>
        /// <returns>IActionResult</returns>
        [HttpGet("{type}/{id}")]
        public async Task<IActionResult> Get(string type, int id)
        {
            List<Category> categories = await _categoryService.Get(type, id);
            return Ok(categories);
        }

        /// <summary>
        /// Thêm thể loại
        /// </summary>
        /// <param name="category">Thông tin thể loại</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] CategoryDto categoryDto)
        {
            int categoryId = await _categoryService.Insert(categoryDto);
            return Ok(categoryId);
        }

        /// <summary>
        /// Cập nhật thông tin thể loại
        /// </summary>
        /// <param name="id">ID thể loại</param>
        /// <param name="category">Thông tin cập nhật</param>
        /// <returns>IActionResult</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryDto categoryDto)
        {
            await _categoryService.Update(id, categoryDto);
            return Ok();
        }

        /// <summary>
        /// Xoá thể loại
        /// </summary>
        /// <param name="id">ID thể loại</param>
        /// <returns>IActionResult</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.Delete(id);
            return Ok();
        }
    }
}