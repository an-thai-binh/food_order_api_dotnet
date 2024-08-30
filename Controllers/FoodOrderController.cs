using FoodOrderApi.Dto;
using FoodOrderApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace FoodOrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodOrderController : ControllerBase
    {
        private readonly YwnacrjeAfoodContext _context;

        public FoodOrderController(YwnacrjeAfoodContext context)
        {
            _context = context;
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
            try
            {
                switch (type)
                {
                    case "all":
                        {
                            var orders = await _context.Foodorders.Include(o => o.Customer).ToListAsync();
                            if (orders.Count == 0)
                            {
                                return NotFound();
                            }
                            return Ok(orders);
                        }
                    case "id":
                        {
                            var orders = await _context.Foodorders.Include(o => o.Customer).Where(o => o.OrderId == id).ToListAsync();
                            if (orders.Count == 0)
                            {
                                return NotFound();
                            }
                            return Ok(orders);
                        }
                    case "customer":
                        {
                            var orders = await _context.Foodorders.Include(o => o.Customer).Where(o => o.CustomerId == id).ToListAsync();
                            if (orders.Count == 0)
                            {
                                return NotFound();
                            }
                            return Ok(orders);
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
        /// Thêm đơn hàng
        /// </summary>
        /// <param name="foodOrderDto">Thông tin đơn hàng</param>
        /// <returns>Task<IActionResult></returns>
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] FoodOrderDto foodOrderDto)
        {
            try
            {
                if (foodOrderDto == null)
                {
                    return BadRequest("Order is NULL");
                }
                var foodOrder = new Foodorder
                {
                    CustomerId = foodOrderDto.CustomerId,
                    OrderName = foodOrderDto.OrderName,
                    OrderEmail = foodOrderDto.OrderEmail,
                    OrderPhoneNumber = foodOrderDto.OrderPhoneNumber,
                    OrderAddress = foodOrderDto.OrderAddress,
                    OrderTime = foodOrderDto.OrderTime,
                    TotalPrice = foodOrderDto.TotalPrice,
                    Status = foodOrderDto.Status
                };
                await _context.AddAsync(foodOrder);
                await _context.SaveChangesAsync();
                return Ok(foodOrder.OrderId);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
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
            try
            {
                var existOrder = await _context.Foodorders.FindAsync(id);
                if(existOrder == null)
                {
                    return BadRequest("Order is NULL");
                }
                existOrder.CustomerId = foodOrderDto.CustomerId;
                existOrder.OrderName = foodOrderDto.OrderName;
                existOrder.OrderEmail = foodOrderDto.OrderEmail;
                existOrder.OrderPhoneNumber = foodOrderDto.OrderPhoneNumber;
                existOrder.OrderAddress = foodOrderDto.OrderAddress;
                existOrder.OrderTime = foodOrderDto.OrderTime;
                existOrder.TotalPrice = foodOrderDto.TotalPrice;
                existOrder.Status = foodOrderDto.Status;
                _context.Update(existOrder);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                var errorMessage = $"Error: {e.Message}";
                if (e.InnerException != null)
                {
                    errorMessage += $" Inner Exception: {e.InnerException.Message}";
                }
                return StatusCode(500, errorMessage);
            }
        }

        /// <summary>
        /// Xoá đơn hàng
        /// </summary>
        /// <param name="id">ID đơn hàng</param>
        /// <returns>Task<IActionResult></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existOrder = await _context.Foodorders.FindAsync(id);
                if (existOrder == null)
                {
                    return NotFound($"Order with ID {id} not found.");
                }
                _context.Foodorders.Remove(existOrder);
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
