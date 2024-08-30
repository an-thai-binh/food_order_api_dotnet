using FoodOrderApi.Dto;
using FoodOrderApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly YwnacrjeAfoodContext _context;

        public OrderDetailController(YwnacrjeAfoodContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lấy (danh sách) chi tiết đơn hàng
        /// </summary>
        /// <param name="type">all: lấy toàn bộ | id: lấy theo id | order: lấy theo id đơn hàng | customer: lấy theo id khách hàng</param>
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
                            var details = await _context.Orderdetails.Include(d => d.Food).ToListAsync();
                            if (details.Count == 0)
                            {
                                return NotFound();
                            }
                            return Ok(details);
                        }
                    case "id":
                        {
                            var details = await _context.Orderdetails.Include(d => d.Food).Where(d => d.DetailId == id).ToListAsync();
                            if (details.Count == 0)
                            {
                                return NotFound();
                            }
                            return Ok(details);
                        }
                    case "order":
                        {
                            var details = await _context.Orderdetails.Include(d => d.Food).Where(d => d.OrderId == id).ToListAsync();
                            if (details.Count == 0)
                            {
                                return NotFound();
                            }
                            return Ok(details);
                        }
                    case "customer":
                        {
                            var details = await _context.Orderdetails.Include(d => d.Food).Where(d => d.Order.CustomerId == id).ToListAsync();
                            if (details.Count == 0)
                            {
                                return NotFound();
                            }
                            return Ok(details);
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
        /// Thêm chi tiết đơn hàng
        /// </summary>
        /// <param name="detailDto">Thông tin chi tiết</param>
        /// <returns>Task<IActionResult></returns>
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] OrderDetailDto detailDto)
        {
            try
            {
                if (detailDto == null)
                {
                    return BadRequest("Detail is NULL");
                }
                var detail = new Orderdetail
                {
                    OrderId = detailDto.OrderId,
                    FoodId = detailDto.FoodId,
                    Quantity = detailDto.Quantity
                };
                await _context.AddAsync(detail);
                await _context.SaveChangesAsync();
                return Ok(detail.DetailId);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Cập nhật thông tin chi tiết
        /// </summary>
        /// <param name="id">id chi tiết đơn hàng</param>
        /// <param name="detailDto">Thông tin cập nhật</param>
        /// <returns>Task<IActionResult></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderDetailDto detailDto)
        {
            try
            {
                if (detailDto == null)
                {
                    return BadRequest("Detail is NULL");
                }
                var existDetail = await _context.Orderdetails.FindAsync(id);
                if (existDetail == null)
                {
                    return NotFound($"Detail with ID {id} not found");
                }
                existDetail.OrderId = detailDto.OrderId;
                existDetail.FoodId = detailDto.FoodId;
                existDetail.Quantity = detailDto.Quantity;
                _context.Update(existDetail);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Xoá chi tiết
        /// </summary>
        /// <param name="type">id: xoá theo id | customer: xoá theo id khách hàng</param>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpDelete("{type}/{id}")]
        public async Task<IActionResult> Delete(string type, int id)
        {
            try
            {
                switch(type)
                {
                    case "id":
                        {
                            var existDetail = await _context.Orderdetails.FindAsync(id);
                            if (existDetail == null)
                            {
                                return NotFound($"Detail with ID {id} not found");
                            }
                            _context.Orderdetails.Remove(existDetail);
                            await _context.SaveChangesAsync();
                            return Ok();
                        }
                    case "customer":
                        {
                            var existDetails = await _context.Orderdetails.Where(d => d.Order.CustomerId == id).ToListAsync();
                            if(existDetails.Count == 0)
                            {
                                return NotFound($"Detail with customer ID {id} not found");
                            }
                            _context.Orderdetails.RemoveRange(existDetails);
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
