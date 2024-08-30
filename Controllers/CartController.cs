using FoodOrderApi.Dto;
using FoodOrderApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly YwnacrjeAfoodContext _context;

        public CartController(YwnacrjeAfoodContext context)
        {
            _context = context;
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
            try
            {
                switch (type)
                {
                    case "all":
                        {
                            var carts = await _context.Carts.Include(c => c.Customer).ToListAsync();
                            if (carts.Count == 0)
                            {
                                return NotFound();
                            }
                            return Ok(carts);
                        }
                    case "id":
                        {
                            var carts = await _context.Carts.Include(c => c.Customer).Where(c => c.CartId == id).ToListAsync();
                            if (carts.Count == 0)
                            {
                                return NotFound();
                            }
                            return Ok(carts);
                        }
                    case "customer":
                        {
                            var carts = await _context.Carts.Include(c => c.Customer).Where(c => c.CustomerId == id).ToListAsync();
                            if (carts.Count == 0)
                            {
                                return NotFound();
                            }
                            return Ok(carts);
                        }
                    default:
                        {
                            return BadRequest();
                        }
                }
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Thêm giỏ hàng
        /// </summary>
        /// <param name="cartDto">Thông tin giỏ hàng</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] CartDto cartDto)
        {
            try
            {
                if(cartDto == null)
                {
                    return BadRequest("Cart is NULL");
                }
                var cart = new Cart
                {
                    CustomerId = cartDto.CustomerId
                };
                await _context.AddAsync(cart);
                await _context.SaveChangesAsync();
                return Ok(cart.CartId);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Xoá giỏ hàng
        /// </summary>
        /// <param name="id">id giỏ hàng</param>
        /// <returns>IActionResult</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existCart = await _context.Carts.FindAsync(id);
                if (existCart == null)
                {
                    return NotFound($"Cart with ID {id} not found.");
                }
                _context.Carts.Remove(existCart);
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
