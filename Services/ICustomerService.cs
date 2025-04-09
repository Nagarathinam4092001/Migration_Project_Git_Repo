using Migration_Project.DTOs;
using Migration_Project.Models;

namespace Migration_Project.Services
{
    public interface ICustomerService
    {
        Task<List<CustomerDTO>> GetAllAsync();
        Task<CustomerDTO> FindByIdAsync(long id);
        Task<bool> AddCustomerAsync(CustomerDTO customer);
        Task<bool> UpdateCustomerAsync(CustomerDTO customer);
        Task<bool> DeleteCustomerAsync(long Id);
    }
}
