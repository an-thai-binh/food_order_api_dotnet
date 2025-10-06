using FoodOrderApi.Dto;
using FoodOrderApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderApi.Repositories
{
    public class CustomerRepository
    {
        private readonly AppDbContext _dbContext;

        public CustomerRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Customer>> GetAll()
        {
            return await _dbContext.Customers.ToListAsync();
        }

        public async Task<Customer?> ShowById(int id)
        {
            return await _dbContext.Customers.FirstOrDefaultAsync(f => f.CustomerId == id);
        }

        public async Task<Customer?> ShowByUsername(string value)
        {
            return await _dbContext.Customers.FirstOrDefaultAsync(f => f.Username == value);
        }

        public async Task<Customer?> ShowByEmail(string value)
        {
            return await _dbContext.Customers.FirstOrDefaultAsync(f => f.Email == value);
        }

        public async Task<Customer?> ShowByEmailAndPassword(string email, string password)
        {
            return await _dbContext.Customers.FirstOrDefaultAsync(f => f.Email == email && f.Password == password); ;
        }

        public async Task<Customer> Save(Customer customer)
        {
            await _dbContext.AddAsync(customer);
            await _dbContext.SaveChangesAsync();
            return customer;
        }

        public async Task Update(int id, CustomerDto customerDto)
        {
            Customer? customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.CustomerId == id);
            if(customer != null)
            {
                customer.Username = customerDto.Username;
                customer.Password = customerDto.Password;
                customer.FullName = customerDto.FullName;
                customer.PhoneNumber = customerDto.PhoneNumber;
                customer.Email = customerDto.Email;
                customer.Address = customerDto.Address;
                customer.BirthDate = DateOnly.FromDateTime(customerDto.BirthDate);
                customer.Gender = (customerDto.Gender ? 1UL : 0UL);  // nữ 1 - nam 0
                customer.Role = customerDto.Role;
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Customer? customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.CustomerId == id);
            if (customer != null)
            {
                _dbContext.Remove(customer);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
