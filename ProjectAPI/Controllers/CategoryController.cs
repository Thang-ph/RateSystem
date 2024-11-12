using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectAPI.Models;

namespace ProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        ProjectPRN231Context context = new ProjectPRN231Context();
        [HttpGet]
        public IActionResult Get()
        {
            var categories = context.Categories.ToList();
            return Ok(categories);
        }
        [HttpGet("{id}")]
        public IActionResult GetByID(int id)
        {
            var category = context.Categories.FirstOrDefault(x => x.CategoryId == id);
            if (category != null)
            {
                return Ok(category);
            }
            return BadRequest();
        }
        [HttpPost]
        public IActionResult Insert([FromBody] Category category)
        {
            if (ModelState.IsValid)
            {
                context.Add(category);
                context.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Category category)
        {
            var c = context.Categories.FirstOrDefault(x => x.CategoryId == id);
            if (c == null)
            {
                return NotFound("Category not found");
            }
            if (ModelState.IsValid)
            {
                c.CategoryName = category.CategoryName;
                c.Description = category.Description;
                context.SaveChanges();
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var c = context.Categories.Include(x=>x.Products).FirstOrDefault(x => x.CategoryId == id);
            if (c != null)
            {
                if (!c.Products.Any())
                {
                    context.Categories.Remove(c);
                    context.SaveChanges();
                    return Ok();
                }
                return BadRequest("Category has product");
            }
            return NotFound("Cannot find category");
        }
    }
}
