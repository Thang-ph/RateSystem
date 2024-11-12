using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectAPI.Mappers;
using ProjectAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProjectPRN231Context _context=new ProjectPRN231Context();
        private readonly IMapper _mapper;

        public ProductController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var products = _context.Products.Include(p => p.Category).OrderBy(x=>x.Price).ToList();
            var productDtos = _mapper.Map<List<ProductDTO>>(products);
            return Ok(productDtos);
        }
        [HttpGet("categoryID")]
        public IActionResult GetByCategory(int categoryID)
        {
            var products = _context.Products.Include(p => p.Category).OrderBy(x=>x.Price).ToList();

            if (categoryID != 0)
            {
                products = products.Where(x => x.CategoryId == categoryID).ToList();
            }
            var productDtos = _mapper.Map<List<ProductDTO>>(products);
            return Ok(productDtos);
        }
        [HttpGet("categoryID/minPrice/maxPrice")]
        public IActionResult GetByCategoryByPrice(int categoryID, int? minPrice = 0, int? maxPrice = 0)
        {
            // Lấy tất cả sản phẩm và bao gồm thông tin Category
            var products = _context.Products.Include(p => p.Category).AsQueryable();

            // Lọc theo categoryID nếu có
            if (categoryID != 0)
            {
                products = products.Where(x => x.CategoryId == categoryID);
            }

            if (minPrice.HasValue)
            {
                products = products.Where(x => x.Price >= minPrice);
            }

            if (maxPrice.HasValue)
            {
                products = products.Where(x => x.Price <= maxPrice);
            }

            // Sắp xếp sản phẩm theo giá tăng dần
            products = products.OrderBy(x => x.Price);

            // Chuyển đổi dữ liệu sang DTO trước khi trả về
            var productDtos = _mapper.Map<List<ProductDTO>>(products.ToList());
            return Ok(productDtos);
        }

        [HttpGet("categoryID/sortOption")]
        public IActionResult GetByCategoryOrder(int categoryID, int sortOption = 1)
        {
            var products = _context.Products.Include(p => p.Category).AsQueryable();

            // Lọc theo categoryID nếu có
            if (categoryID != 0)
            {
                products = products.Where(x => x.CategoryId == categoryID);
            }

            // Sắp xếp theo Price dựa trên sortOption
            if (sortOption == 2)
            {
                products = products.OrderByDescending(x => x.Price);
            }
            else
            {
                products = products.OrderBy(x => x.Price);
            }

            // Map danh sách sản phẩm sang ProductDTO
            var productDtos = _mapper.Map<List<ProductDTO>>(products.ToList());
            return Ok(productDtos);
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var product = _context.Products.Include(p => p.Category).FirstOrDefault(x => x.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            var productDto = _mapper.Map<ProductDTO>(product);
            return Ok(productDto);
        }

        [HttpPost]
        public IActionResult Insert([FromBody] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var p = _context.Products.FirstOrDefault(x => x.ProductId == id);
            if (p == null)
            {
                return NotFound();
            }

            // Map dữ liệu mới từ Product vào entity
            p.ProductName = product.ProductName;
            p.CategoryId = product.CategoryId;
            p.Description = product.Description;
            p.Image = product.Image;
            p.Price = product.Price;

            _context.Products.Update(p);
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var p = _context.Products.FirstOrDefault(x => x.ProductId == id);
            if (p == null)
            {
                return NotFound();
            }

            _context.Products.Remove(p);
            _context.SaveChanges();
            return Ok();
        }
    }
}
