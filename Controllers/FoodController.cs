using FoodOrderApi.Dto;
using FoodOrderApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly YwnacrjeAfoodContext _context;

        public FoodController(YwnacrjeAfoodContext context)
        {
            _context = context;
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
            try
            {
                switch (type)
                {
                    case "all":
                        {
                            var foods = await _context.Foods.Include(f => f.Category).ToListAsync();
                            if (foods.Count == 0)
                            {
                                return NotFound();
                            }
                            return Ok(foods);
                        }
                    case "id":
                        {
                            var foods = await _context.Foods.Include(f => f.Category).Where(f => f.FoodId == id).ToListAsync();
                            if (foods.Count == 0)
                            {
                                return NotFound();
                            }
                            return Ok(foods);
                        }
                    case "category":
                        {
                            var foods = await _context.Foods.Include(f => f.Category).Where(f => f.CategoryId == id).ToListAsync();
                            if (foods.Count == 0)
                            {
                                return NotFound();
                            }
                            return Ok(foods);
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
        /// Tìm kiếm món ăn theo tên
        /// </summary>
        /// <param name="query">từ khoá</param>
        /// <returns>IActionResult</returns>
        [HttpGet("search/{query}")]
        public async Task<IActionResult> Search(string query)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    return BadRequest("Search query cannot be empty");
                }
                var lowerQuery = query.ToLower();
                var foods = await _context.Foods.Include(f => f.Category).Where(f => (f.FoodName ?? "").ToLower().Contains(lowerQuery)).ToListAsync();
                return Ok(foods);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Tìm kiếm món ăn theo tên
        /// </summary>
        /// <param name="query">từ khoá</param>
        /// <returns>IActionResult</returns>
        [HttpGet("top/{top}")]
        public async Task<IActionResult> Search(int top)
        {
            try
            {
                var foods = await _context.Foods.Include(f => f.Category).Select(f => new
                {
                    Food = f,
                    TotalQuantity = _context.Orderdetails.Where(od => od.FoodId == f.FoodId).Sum(od => od.Quantity)
                }).OrderByDescending(x => x.TotalQuantity)
                .Take(top)
                .ToListAsync();
                return Ok(foods);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Thêm món ăn
        /// </summary>
        /// <param name="foodDto">Thông tin món ăn</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] FoodDto foodDto)
        {
            try
            {
                if(foodDto == null)
                {
                    return BadRequest("Food is NULL");
                }
                var food = new Food
                {
                    CategoryId = foodDto.CategoryId,
                    FoodName = foodDto.FoodName,
                    Description = foodDto.Description,
                    Price = foodDto.Price,
                    ImgUrl = foodDto.ImgUrl
                };
                await _context.AddAsync(food);
                await _context.SaveChangesAsync();
                return Ok(food.FoodId);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
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
            try
            {
                if (foodDto == null)
                {
                    return BadRequest("Food is NULL");
                }
                var existFood = await _context.Foods.FindAsync(id);
                if(existFood == null)
                {
                    return NotFound($"Food with ID {id} not found");
                }
                existFood.CategoryId = foodDto.CategoryId;
                existFood.FoodName = foodDto.FoodName;
                existFood.Description = foodDto.Description;
                existFood.Price = foodDto.Price;
                existFood.ImgUrl = foodDto.ImgUrl;
                _context.Update(existFood);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Xoá món ăn
        /// </summary>
        /// <param name="id">ID món ăn</param>
        /// <returns>IActionResult</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existFood = await _context.Foods.FindAsync(id);
                if (existFood == null)
                {
                    return NotFound($"Food with ID {id} not found");
                }
                _context.Foods.Remove(existFood);
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
