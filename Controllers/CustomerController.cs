using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Migration_Project.DTOs;
using Migration_Project.Models;
using Migration_Project.Services;
using Migration_Project.Services.ASPWebFormDapperDemo.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Migration_Project.Controllers
{
    /// <summary>
    /// Controller for managing customer information
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        /// <summary>
        /// Initializes a new instance of the CustomerController
        /// </summary>
        /// <param name="customerService">Service for customer operations</param>
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Retrieves all customers with optional image inclusion
        /// </summary>
        /// <param name="includeImages">When true, includes customer images in the response</param>
        /// <returns>A collection of customers</returns>
        /// <response code="200">Returns the list of customers</response>
        /// <response code="404">If no customers are found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CustomerDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetAllAsync()
        {
            try
            {
                var customers = await _customerService.GetAllAsync();
                if (customers == null || customers.Count == 0)
                {
                    return NotFound(new { message = "No customers found" });
                }
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "Error retrieving customers", error = ex.Message });
            }
        }

        /// <summary>
        /// Gets a specific customer by ID
        /// </summary>
        /// <param name="id">The ID of the customer to retrieve</param>
        /// <returns>The requested customer</returns>
        /// <response code="200">Returns the requested customer</response>
        /// <response code="404">If the customer is not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("get/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CustomerDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CustomerDTO>> GetCustomerByIdAsync(long id)
        {
            try
            {
                var customer = await _customerService.FindByIdAsync(id);
                if (customer != null)
                {
                    return Ok(customer);
                }
                return NotFound(new { message = "Customer not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error retrieving customer", error = ex.Message });
            }
        }

        /// <summary>
        /// Creates a new customer
        /// </summary>
        /// <param name="customerDto">The customer information to add</param>
        /// <returns>Result of the operation with the new customer ID</returns>
        /// <response code="200">Customer was successfully created</response>
        /// <response code="400">If the request is invalid or creation failed</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddCustomerAsync([FromBody] CustomerDTO customerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _customerService.AddCustomerAsync(customerDto);
                if (result)
                {
                    return Ok(new { message = "Successfully added the record" });
                }
                return BadRequest(new { message = "Failed to add the record" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error adding customer", error = ex.Message });
            }
        }

        /// <summary>
        /// Updates an existing customer
        /// </summary>
        /// <param name="id">The ID of the customer to update</param>
        /// <param name="customerDto">The updated customer information</param>
        /// <returns>Result of the update operation</returns>
        /// <response code="200">Customer was successfully updated</response>
        /// <response code="400">If the request is invalid or update failed</response>
        /// <response code="404">If the customer is not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut("update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateCustomerAsync(long id, [FromBody] CustomerDTO customerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                customerDto.CustomerID = id;
                var result = await _customerService.UpdateCustomerAsync(customerDto);
                if (result)
                    return Ok(new { message = "Successfully updated the record" });
                return NotFound(new { message = "Customer not found or failed to update the record" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error updating customer", error = ex.Message });
            }
        }

        /// <summary>
        /// Deletes a customer by ID
        /// </summary>
        /// <param name="id">The ID of the customer to delete</param>
        /// <returns>Result of the delete operation</returns>
        /// <response code="200">Customer was successfully deleted</response>
        /// <response code="400">If deletion failed</response>
        /// <response code="404">If the customer is not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteCustomerAsync(long id)
        {
            try
            {
                var result = await _customerService.DeleteCustomerAsync(id);
                if (result)
                    return Ok(new { message = "Successfully deleted the record" });
                return NotFound(new { message = "Customer not found or failed to delete record" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error deleting customer", error = ex.Message });
            }
        }

        /// <summary>
        /// Edits a customer by ID (alternative to PUT update)
        /// </summary>
        /// <param name="id">The ID of the customer to edit</param>
        /// <param name="customerDto">The updated customer information</param>
        /// <returns>Result of the edit operation</returns>
        /// <response code="200">Customer was successfully edited</response>
        /// <response code="400">If the request is invalid or edit failed</response>
        /// <response code="404">If the customer is not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost("edit/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> EditCustomerAsync(long id, [FromBody] CustomerDTO customerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                customerDto.CustomerID = id;
                var result = await _customerService.UpdateCustomerAsync(customerDto);
                if (result)
                    return Ok(new { message = "Successfully edited the record" });
                return NotFound(new { message = "Customer not found or failed to edit record" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error editing customer", error = ex.Message });
            }
        }

        /// <summary>
        /// Handles cancellation of customer edits
        /// </summary>
        /// <returns>Confirmation of edit cancellation</returns>
        /// <response code="200">Edit was successfully cancelled</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost("cancelEdit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult CancelEdit()
        {
            try
            {
                // Logic to handle cancel edit, if needed
                return Ok(new { message = "Edit cancelled" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error cancelling edit", error = ex.Message });
            }
        }

        /// <summary>
        /// Handles page index changes for pagination
        /// </summary>
        /// <param name="newIndex">The new page index</param>
        /// <returns>Confirmation of page index change</returns>
        /// <response code="200">Page index was successfully changed</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost("pageIndexChange")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult PageIndexChange(int newIndex)
        {
            try
            {
                // Logic to handle page index change, if needed
                return Ok(new { message = "Page index changed", index = newIndex });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error changing page index", error = ex.Message });
            }
        }
    }
}
