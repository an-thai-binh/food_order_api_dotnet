using FoodOrderApi.Dto;
using FoodOrderApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly YwnacrjeAfoodContext _context;

        public CartItemController(YwnacrjeAfoodContext context)
        {
            _context = context;
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
            try
            {
                switch (type)
                {
                    case "all":
                        {
                            var items = await _context.Cartitems.Include(c => c.Cart).Include(c => c.Food).ToListAsync();
                            if (items.Count == 0)
                            {
                                return NotFound();
                            }
                            return Ok(items);
                        }
                    case "id":
                        {
                            var items = await _context.Cartitems.Include(c => c.Cart).Include(c => c.Food).Where(c => c.FoodId == id).ToListAsync();
                            if (items.Count == 0)
                            {
                                return NotFound();
                            }
                            return Ok(items);
                        }
                    case "cart":
                        {
                            var items = await _context.Cartitems.Include(c => c.Cart).Include(c => c.Food).Where(c => c.CartId == id).ToListAsync();
                            if (items.Count == 0)
                            {
                                return NotFound();
                            }
                            return Ok(items);
                        }
                    case "customer":
                        {
                            var items = await _context.Cartitems.Include(c => c.Cart).Include(c => c.Food).Where(c => c.Cart.CustomerId == id).ToListAsync();
                            if (items.Count == 0)
                            {
                                return NotFound();
                            }
                            return Ok(items);
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
        /// Thêm chi tiết giỏ hàng
        /// </summary>
        /// <param name="cartItemDto">thông tin món ăn</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] CartItemDto cartItemDto)
        {
            try
            {
                if(cartItemDto == null)
                {
                    return BadRequest("CartItem is NULL");
                }
                var existItem = await _context.Cartitems.FirstOrDefaultAsync(c => c.CartId == cartItemDto.CartId && c.FoodId == cartItemDto.FoodId);
                if(existItem != null)
                {
                    existItem.Quantity = existItem.Quantity + cartItemDto.Quantity;
                    _context.Update(existItem);
                    await _context.SaveChangesAsync();
                    return Ok(existItem.ItemId);
                }
                var cartItem = new Cartitem
                {
                    CartId = cartItemDto.CartId,
                    FoodId = cartItemDto.FoodId,
                    Quantity = cartItemDto.Quantity
                };
                await _context.AddAsync(cartItem);
                await _context.SaveChangesAsync();
                return Ok(cartItem.ItemId);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Cập nhật số lượng của món ăn
        /// </summary>
        /// <param name="id">id chi tiết</param>
        /// <param name="quantity">số lượng</param>
        /// <returns>IActionResult</returns>
        [HttpPut("{id}/{quantity}")]
        public async Task<IActionResult> ChangeQuantity(int id, int quantity)
        {
            try
            {
                var existItem = await _context.Cartitems.FindAsync(id);
                if(existItem == null)
                {
                    return NotFound($"CartItem with ID {id} not found");
                }
                existItem.Quantity = quantity;
                _context.Update(existItem);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Xoá món ăn trong giỏ hàng
        /// </summary>
        /// <param name="id">id chi tiết</param>
        /// <returns>IActionResult</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existItem = await _context.Cartitems.FindAsync(id);
                if (existItem == null)
                {
                    return NotFound($"CartItem with ID {id} not found");
                }
                _context.Cartitems.Remove(existItem);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
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
            try
            {
                switch (type)
                {
                    case "customer":
                        {
                            var existItems = await _context.Cartitems.Where(c => c.Cart.CustomerId == id).ToListAsync();
                            if(existItems.Count == 0)
                            {
                                return NotFound($"Item with customer ID {id} not found");
                            }
                            _context.Cartitems.RemoveRange(existItems);
                            await _context.SaveChangesAsync();
                            return Ok();
                        }
                    case "cart":
                        {
                            var existItems = await _context.Cartitems.Where(c => c.CartId == id).ToListAsync();
                            if (existItems.Count == 0)
                            {
                                return NotFound($"Item with customer ID {id} not found");
                            }
                            _context.Cartitems.RemoveRange(existItems);
                            await _context.SaveChangesAsync();
                            return Ok();
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
    }
}
