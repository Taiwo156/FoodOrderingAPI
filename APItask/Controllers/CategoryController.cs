using APItask.Core;
using APItask.Core.Models;
using APItask.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace APItask.Properties
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly EssentialProductsDbContext dbContext;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(EssentialProductsDbContext dbContext, ILogger<CategoryController> logger)
        {
            this.dbContext = dbContext;
            _logger = logger;
        }

        // GET: api/<CategoryController>
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var categories = await dbContext.Category.ToListAsync();
            _logger.LogInformation("Retrieved all categories.");
            return Ok(categories);
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(short id)
        {
            var category = await dbContext.Category.FindAsync(id);
            if (category == null)
            {
                _logger.LogWarning("Category with ID {Id} not found.", id);
                return NotFound();
            }

            _logger.LogInformation("Retrieved category with ID {Id}.", id);
            return Ok(category);
        }

        // POST api/<CategoryController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] string value)
        {
            var entityToAdd = new Category() { Name = value, IsActive = true };
            await dbContext.Category.AddAsync(entityToAdd);
            await dbContext.SaveChangesAsync();
            _logger.LogInformation("Created new category with ID {Id}.", entityToAdd.Id);
           // return new CreatedResult("Get", entityToAdd.Id);
            return Ok(new { message = "Category created successfully." });
        }

        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(short id, [FromBody] string value)
        {
            var entityInDb = await dbContext.Category.FindAsync(id);
            if (entityInDb == null)
            {
                _logger.LogWarning("Category with ID {Id} not found for update.", id);
                return NotFound();
            }

            entityInDb.Name = value;
            dbContext.Update(entityInDb);
            await dbContext.SaveChangesAsync();
            _logger.LogInformation("Updated category with ID {Id}.", id);
            return Ok(new { message = "Category updated successfully." });
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(short id)
        {
            var entityInDb = await dbContext.Category.FindAsync(id);
            if (entityInDb == null)
            {
                _logger.LogWarning("Category with ID {Id} not found for deletion.", id);
                return NotFound();
            }

            dbContext.Category.Remove(entityInDb);
            await dbContext.SaveChangesAsync();
            _logger.LogInformation("Deleted category with ID {Id}.", id);
            return Ok(new { message = "Category deleted successfully." });
        }
    }
}
