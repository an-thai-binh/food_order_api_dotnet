using FoodOrderApi.Dto;
using FoodOrderApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace FoodOrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly YwnacrjeAfoodContext _context;

        public CustomerController(YwnacrjeAfoodContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lấy (danh sách) khách hàng
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var customers = await _context.Customers.ToListAsync();
                if(customers.Count == 0)
                {
                    return NotFound();
                }
                return Ok(customers);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Lấy khách hàng
        /// </summary>
        /// <param name="type">id - lấy theo id khách hàng | username - lấy theo username | email - lấy theo email</param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet("{type}/{value}")]
        public async Task<IActionResult> Get(string type, string value){
            try
            {
                switch (type)
                {
                    case "id":
                        {
                            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == int.Parse(value));
                            if(customer == null) {
                                return NotFound();
                            }
                            return Ok(customer);
                        }
                    case "username":
                        {
                            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Username.Equals(value));
                            if (customer == null)
                            {
                                return NotFound();
                            }
                            return Ok(customer);
                        }
                    case "email":
                        {
                            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email.Equals(value));
                            if (customer == null)
                            {
                                return NotFound();
                            }
                            return Ok(customer);
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
        /// Đăng nhập
        /// </summary>
        /// <param name="loginRequest">Thông tin đăng nhập (email, password)</param>
        /// <returns>IActionResult</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                if(loginRequest == null)
                {
                    return BadRequest("Login Request is NULL");
                }
                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email.Equals(loginRequest.Email) && c.Password.Equals(loginRequest.Password));
                if(customer == null)
                {
                    return NotFound();
                }
                return Ok(customer);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Đăng ký
        /// </summary>
        /// <param name="customerDto">Thông tin khách hàng</param>
        /// <returns>IActionResult</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CustomerDto customerDto)
        {
            try
            {
                if (customerDto == null)
                {
                    return BadRequest("Customer is NULL");
                }
                var existCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Username.Equals(customerDto.Username));
                if(existCustomer != null)
                {
                    return BadRequest("Customer already exist");
                }
                var customer = new Customer
                {
                    Username = customerDto.Username,
                    Password = customerDto.Password,
                    FullName = customerDto.FullName,
                    PhoneNumber = customerDto.PhoneNumber,
                    Email = customerDto.Email,
                    Address = customerDto.Address,
                    BirthDate = DateOnly.FromDateTime(customerDto.BirthDate),
                    Gender = (customerDto.Gender ? 1UL : 0UL),  // nữ 1 - nam 0
                    Role = customerDto.Role
                };
                await _context.AddAsync(customer);
                await _context.SaveChangesAsync();
                return Ok(customer.CustomerId);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Chỉnh sửa thông tin khách hàng
        /// </summary>
        /// <param name="customerDto">Thông tin khách hàng</param>
        /// <returns>IActionResult</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CustomerDto customerDto)
        {
            try
            {
                if (customerDto == null)
                {
                    return BadRequest("Customer is NULL");
                }
                var existCustomer = await _context.Customers.FindAsync(id);
                if(existCustomer == null)
                {
                    return NotFound($"Customer with ID {id} not found");
                }
                existCustomer.Username = customerDto.Username;
                existCustomer.Password = customerDto.Password;
                existCustomer.FullName = customerDto.FullName;
                existCustomer.PhoneNumber = customerDto.PhoneNumber;
                existCustomer.Email = customerDto.Email;
                existCustomer.Address = customerDto.Address;
                existCustomer.BirthDate = DateOnly.FromDateTime(customerDto.BirthDate);
                existCustomer.Gender = (customerDto.Gender ? 1UL : 0UL);  // nữ 1 - nam 0
                existCustomer.Role = customerDto.Role;
                _context.Update(existCustomer);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Xoá thông tin khách hàng
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>IActionResult</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existCustomer = await _context.Customers.FindAsync(id);
                if (existCustomer == null)
                {
                    return NotFound($"Customer with ID {id} not found");
                }
                _context.Remove(existCustomer);
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
