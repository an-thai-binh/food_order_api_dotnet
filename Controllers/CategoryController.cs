using FoodOrderApi.Dto;
using FoodOrderApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly YwnacrjeAfoodContext _context;

        public CategoryController(YwnacrjeAfoodContext context)
        {
            _context = context;
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
            try
            {
                switch (type)
                {
                    case "all":
                        {
                            var categories = await _context.Categories.ToListAsync();
                            if (categories.Count == 0)
                            {
                                return NotFound();
                            }
                            return Ok(categories);
                        }
                    case "id":
                        {
                            var categories = await _context.Categories.Where(c => c.CategoryId == id).ToListAsync();
                            if (categories.Count == 0)
                            {
                                return NotFound();
                            }
                            return Ok(categories);
                        }
                    default:
                        {
                            return BadRequest();
                        }
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Thêm thể loại
        /// </summary>
        /// <param name="category">Thông tin thể loại</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] CategoryDto categoryDto)
        {
            try
            {
                if (categoryDto == null)
                {
                    return BadRequest("Category is NULL");
                }
                var category = new Category
                {
                    CategoryName = categoryDto.CategoryName,
                    CategoryImgUrl = categoryDto.CategoryImgUrl
                };
                await _context.AddAsync(category);
                await _context.SaveChangesAsync();
                return Ok(category.CategoryId);
            } 
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
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
            try
            {
                if (categoryDto == null)
                {
                    return BadRequest("Category is NULL.");
                }
                var existCategory = await _context.Categories.FindAsync(id);
                if (existCategory == null)
                {
                    return NotFound($"Category with ID {id} not found");
                }
                existCategory.CategoryName = categoryDto.CategoryName;
                existCategory.CategoryImgUrl = categoryDto.CategoryImgUrl;
                _context.Categories.Update(existCategory);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Xoá thể loại
        /// </summary>
        /// <param name="id">ID thể loại</param>
        /// <returns>IActionResult</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existCategory = await _context.Categories.FindAsync(id);
                if(existCategory == null)
                {
                    return NotFound($"Category with ID {id} not found.");
                }
                _context.Categories.Remove(existCategory);
                await _context.SaveChangesAsync();
                return Ok();
            } 
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
    }
}
