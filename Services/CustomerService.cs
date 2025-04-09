using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using Migration_Project.DTOs;
using Migration_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Migration_Project.Services
{
    namespace ASPWebFormDapperDemo.Services
    {
        public class CustomerService : ICustomerService
        {
            private readonly ICustomerRepository _repository;
            private readonly IMapper _mapper;

            public CustomerService(ICustomerRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<List<CustomerDTO>> GetAllAsync()
            {
                var customers = await _repository.GetAllAsync();
                var customerDtos = _mapper.Map<List<CustomerDTO>>(customers);

                return customerDtos;
            }

            public async Task<CustomerDTO> FindByIdAsync(long id)
            {
                var customer = await _repository.FindByIdAsync(id);
                if (customer == null) return null;

                var customerDto = _mapper.Map<CustomerDTO>(customer);

                return customerDto;
            }

            public async Task<bool> AddCustomerAsync(CustomerDTO customerDto)
            {
                var customer = _mapper.Map<Customer>(customerDto);
                return await _repository.AddCustomerAsync(customer);
            }

            public async Task<bool> UpdateCustomerAsync(CustomerDTO customerDto)
            {
                var customer = _mapper.Map<Customer>(customerDto);
                return await _repository.UpdateCustomerAsync(customer);
            }

            public async Task<bool> DeleteCustomerAsync(long id)
            {
                return await _repository.DeleteCustomerAsync(id);
            }

        }
    }
}
