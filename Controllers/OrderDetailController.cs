using FoodOrderApi.Dto;
using FoodOrderApi.Models;
using FoodOrderApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly OrderDetailService _detailService;

        public OrderDetailController(OrderDetailService detailService)
        {
            _detailService = detailService;
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
            List<Orderdetail> details = await _detailService.Get(type, id);
            return Ok(details);
        }

        /// <summary>
        /// Thêm chi tiết đơn hàng
        /// </summary>
        /// <param name="detailDto">Thông tin chi tiết</param>
        /// <returns>Task<IActionResult></returns>
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] OrderDetailDto detailDto)
        {
            int detailId = await _detailService.Insert(detailDto);
            return Ok(detailId);
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
            await _detailService.Update(id, detailDto);
            return Ok();
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
            await _detailService.Delete(type, id);
            return Ok();
        }
    }
}
