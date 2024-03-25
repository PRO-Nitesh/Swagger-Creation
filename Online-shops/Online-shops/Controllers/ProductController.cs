using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Online_shops.Models;

namespace Online_shops.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AllContext _dbContext;

        public ProductController(AllContext _Context)
        {
            _dbContext = _Context;   
        }

        [HttpGet]
        [Route ("GetProducts")]
        public async  Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            if(_dbContext.Products == null)
            {
                return NotFound();
            }
            return await _dbContext.Products.ToListAsync();
        }

        
        [HttpGet]
        [Route ("GetProductById")]
        public async Task<ActionResult<Product>> GetProductById(Guid productID)
        {
            if (_dbContext.Products == null)
            {
                return NotFound();
            }
            var product = await _dbContext.Products.FindAsync(productID);
            if(product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        

        [HttpPost]
        [Route("PostProduct")]
        public async Task<ActionResult<Product>> PostProduct(Product prodcut)
        {
            prodcut.productID = Guid.NewGuid();
            _dbContext.Products.Add(prodcut);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProducts), new { productID = prodcut.productID }, prodcut);
        }

        [HttpPut]
        public async Task<IActionResult>PutProduct(Guid productID, Product product)
        {
            if (productID != product.productID)
            {
                return BadRequest();
            }
            _dbContext.Entry(product).State = EntityState.Modified;

            try 
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!productAvailable(productID))
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
        private bool productAvailable(Guid productID)
        {
            return (_dbContext.Products?.Any(x => x.productID == productID)).GetValueOrDefault();
        }

        
        [HttpDelete("{productID}")]
        public async Task<IActionResult>DeleteProduct(Guid productID)
        {
            if(_dbContext.Products == null) 
            {
             return NotFound();
            }
            var product = await _dbContext.Products.FindAsync(productID);
            if(product == null)
            {
                return NotFound();
            }

            _dbContext.Products.Remove(product);

            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        

    }
}
