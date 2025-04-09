using Microsoft.EntityFrameworkCore;
using Migration_Project.Data;
using Migration_Project.Models;

namespace Migration_Project.Services
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDbContext _context;

        public CustomerRepository(CustomerDbContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _context.Customers.AsNoTracking().ToListAsync();
        }

        public async Task<Customer> FindByIdAsync(long id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<bool> AddCustomerAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateCustomerAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCustomerAsync(long id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }
    }
}
