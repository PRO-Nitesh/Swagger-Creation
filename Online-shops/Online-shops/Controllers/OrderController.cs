using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_shops.Models;

namespace Online_shops.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AllContext _dbContext;

        public OrderController(AllContext _Context)
        {
            _dbContext = _Context;
        }

        [HttpGet]
        [Route("GetOder")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrder()
        {
            if (_dbContext.Customers == null)
            {
                return NotFound();
            }
            return await _dbContext.Orders.ToListAsync();
        }


        [HttpGet]
        [Route("GetOrderId")]
        public async Task<ActionResult<Order>> GetOrderById(Guid orderId)
        {
            if (_dbContext.Orders == null)
            {
                return NotFound();
            }
            var order = await _dbContext.Orders.FindAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpPost]
        [Route("PostOrder")]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            order.orderId = Guid.NewGuid();
            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { orderId = order.orderId }, order);
        }


        [HttpPut]
        public async Task<IActionResult> PutOrder(Guid orderId, Order order)
        {
            if (orderId != order.orderId)
            {
                return BadRequest();
            }
            _dbContext.Entry(order).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!productAvailable(orderId))
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
        private bool productAvailable(Guid orderId)
        {
            return (_dbContext.Orders?.Any(x => x.orderId == orderId)).GetValueOrDefault();
        }


        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(Guid orderId)
        {
            if (_dbContext.Orders == null)
            {
                return NotFound();
            }
            var order = await _dbContext.Orders.FindAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            _dbContext.Orders.Remove(order);

            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
