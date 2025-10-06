using FoodOrderApi.Dto;
using FoodOrderApi.Models;
using FoodOrderApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerService _customerService;

        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Lấy (danh sách) khách hàng
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<Customer> customer = await _customerService.Get();
            return Ok(customer);
        }

        /// <summary>
        /// Lấy khách hàng
        /// </summary>
        /// <param name="type">id - lấy theo id khách hàng | username - lấy theo username | email - lấy theo email</param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet("{type}/{value}")]
        public async Task<IActionResult> Show(string type, string value){
            Customer customer = await _customerService.Show(type, value);
            return Ok(customer);
        }

        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <param name="loginRequest">Thông tin đăng nhập (email, password)</param>
        /// <returns>IActionResult</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            Customer customer = await _customerService.Login(loginRequest);
            return Ok(customer);
        }

        /// <summary>
        /// Đăng ký
        /// </summary>
        /// <param name="customerDto">Thông tin khách hàng</param>
        /// <returns>IActionResult</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CustomerDto customerDto)
        {
            int customerId = await _customerService.Register(customerDto);
            return Ok(customerId);
        }

        /// <summary>
        /// Chỉnh sửa thông tin khách hàng
        /// </summary>
        /// <param name="customerDto">Thông tin khách hàng</param>
        /// <returns>IActionResult</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CustomerDto customerDto)
        {
            await _customerService.Update(id, customerDto);
            return Ok();
        }

        /// <summary>
        /// Xoá thông tin khách hàng
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>IActionResult</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _customerService.Delete(id);
            return Ok();
        }
    }
}
