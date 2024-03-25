using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Online_shops.Models;

namespace Online_shops.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly AllContext _dbContext;

        public CustomerController(AllContext _Context)
        {
            _dbContext = _Context;
        }

        [HttpGet]
        [Route("GetCustomer")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomer()
        {
            if (_dbContext.Customers == null)
            {
                return NotFound();
            }
            return await _dbContext.Customers.ToListAsync();
        }



        [HttpGet]
        [Route("GetCustomreById")]
        public async Task<ActionResult<Customer>> GetProductById(Guid CustomerId)
        {
            if (_dbContext.Customers == null)
            {
                return NotFound();
            }
            var product = await _dbContext.Customers.FindAsync(CustomerId);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        [Route("PostCustomer")]
        public async Task<ActionResult<Product>> PostCustomer(Customer customer)
        {
            customer.CustomerId = Guid.NewGuid();
            _dbContext.Customers.Add(customer);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomer), new { CustomerId = customer.CustomerId }, customer);
        }

        [HttpPut]
        public async Task<IActionResult>PutCustomer(Guid CustomerId, Customer customer)
        {
            if (CustomerId != customer.CustomerId)
            {
                return BadRequest();
            }
            _dbContext.Entry(customer).State = EntityState.Modified;

            try 
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!productAvailable(CustomerId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok();
            
        }
        private bool productAvailable(Guid CustomerId)
        {
            return (_dbContext.Customers?.Any(x => x.CustomerId == CustomerId)).GetValueOrDefault();
        }


        [HttpDelete("{CustomerId}")]
        public async Task<IActionResult> DeleteCustomer(Guid CustomerId)
        {
            if (_dbContext.Customers == null)
            {
                return NotFound();
            }
            var customer = await _dbContext.Customers.FindAsync(CustomerId);
            if (customer == null)
            {
                return NotFound();
            }

            _dbContext.Customers.Remove(customer);

            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
