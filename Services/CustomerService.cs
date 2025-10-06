using FoodOrderApi.Dto;
using FoodOrderApi.Exceptions;
using FoodOrderApi.Models;
using FoodOrderApi.Repositories;

namespace FoodOrderApi.Services
{
    public class CustomerService
    {
        private readonly CustomerRepository _customerRepo;

        public CustomerService(CustomerRepository customerRepo)
        {
            _customerRepo = customerRepo;
        }

        public async Task<List<Customer>> Get()
        {
            List<Customer> customers = await _customerRepo.GetAll();
            if(customers.Count == 0)
            {
                throw new AppException(StatusCodes.Status404NotFound, "Not found");
            }
            return customers;
        }

        public async Task<Customer> Show(string type, string value)
        {
            Customer? customer = null;
            switch(type)
            {
                case "id":
                    {
                        customer = await _customerRepo.ShowById(int.Parse(value));
                        break;
                    }
                case "username":
                    {
                        customer = await _customerRepo.ShowByUsername(value);
                        break;
                    }
                case "email":
                    {
                        customer = await _customerRepo.ShowByEmail(value);
                        break;
                    }
            }
            if(customer == null)
            {
                throw new AppException(StatusCodes.Status404NotFound, "Not found"); ;
            }
            return customer;
        }
        
        public async Task<Customer> Login(LoginRequest loginRequest)
        {
            if(loginRequest == null)
            {
                throw new AppException(StatusCodes.Status400BadRequest, "Login Request is NULL");
            }
            Customer? customer = await _customerRepo.ShowByEmailAndPassword(loginRequest.Email, loginRequest.Password);
            if(customer == null)
            {
                throw new AppException(StatusCodes.Status404NotFound, "Not found");
            }
            return customer;
        }

        public async Task<int> Register(CustomerDto customerDto)
        {
            if(customerDto == null)
            {
                throw new AppException(StatusCodes.Status400BadRequest, "Customer is NULL");
            }
            Customer? existedCustomer = await _customerRepo.ShowByUsername(customerDto.Username);
            if(existedCustomer != null)
            {
                throw new AppException(StatusCodes.Status400BadRequest, "Customer already exists");
            }
            Customer customer = new Customer
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
            customer = await _customerRepo.Save(customer);
            return customer.CustomerId;
        }

        public async Task Update(int id, CustomerDto customerDto)
        {
            if (customerDto == null)
            {
                throw new AppException(StatusCodes.Status400BadRequest, "Customer is NULL");
            }
            Customer existedCustomer = await _customerRepo.ShowByUsername(customerDto.Username) ?? throw new AppException(StatusCodes.Status404NotFound, $"Customer with ID {id} not found");
            await _customerRepo.Update(id, customerDto);
        }

        public async Task Delete(int id)
        {
            Customer existedCustomer = await _customerRepo.ShowById(id) ?? throw new AppException(StatusCodes.Status404NotFound, $"Customer with ID {id} not found");
            await _customerRepo.Delete(id);
        }
    }
}
