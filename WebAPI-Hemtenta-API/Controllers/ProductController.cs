using System;
using System.Linq;
using API_Web_API_Kurs.Data;
using API_Web_API_Kurs.Models;
using API_Web_API_Kurs.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Web_API_Kurs.Controllers
{
    [Authorize]
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }




        // GET: api/Product/5
        [HttpGet("{id}", Name = "GetProduct")]
        public JsonResult GetProduct(int id)
        {



            var product = _context.Products
                .Select(p => new
                {

                    id = p.Id,
                    name = p.Name,
                    description = p.Description,
                    price = p.Price,
                    imageUrl = p.ImageUrl,
                    urlSlug = p.UrlSlug,
                    categories = p.Categories.Select(c => new
                    {
                        Id = c.CategoryId,
                        Name = c.Category.Name,

                    })

                }).FirstOrDefault(x => x.id == id);


            return new JsonResult(product);

        }



        [HttpGet(Name = "GetProducts")]
        public JsonResult Get()
        {

            var products = _context.Products
                .Select(p => new
                {

                    id = p.Id,
                    name = p.Name,
                    description = p.Description,
                    price = p.Price,
                    imageUrl = p.ImageUrl,
                    urlSlug = p.UrlSlug,
                    categories = p.Categories.Select(c => new
                    {
                        Id = c.CategoryId,
                        Name = c.Category.Name,

                    })

                }).ToList();


            return new JsonResult(products);


        }


        // POST: api/Product
        [Authorize(Policy = "IsAdministrator")]
        [HttpPost]
        public IActionResult Post(CreateProductDto productDto)
        {
            Product clothing = new Product();

            string urlSlug = productDto.Name.ToLower().Replace(" ", "-");

            clothing.Name = productDto.Name;
            clothing.Description = productDto.Description;
            clothing.Price = productDto.Price;
            clothing.ImageUrl = productDto.ImageUrl;
            clothing.UrlSlug = urlSlug;
            try
            {

                _context.Add(clothing);
                _context.SaveChanges();


                if (productDto.CategoriesId != null)
                {

                    foreach (var categoryId in productDto.CategoriesId)
                    {
                        ProductCategory productCategory = new ProductCategory(clothing);

                        var category = _context.Categories.FirstOrDefault(x => x.Id == categoryId);

                        category.Products.Add(productCategory);
                        _context.SaveChanges();


                    }

                }



            }
            catch (Exception e)
            {
                return BadRequest(e.GetBaseException());
            }

            ProductDto product = new ProductDto();

            product.Id = clothing.Id;
            product.Name = clothing.Name;
            product.ImageUrl = clothing.ImageUrl;
            product.Price = clothing.Price;
            product.UrlSlug = clothing.UrlSlug;
            product.Categories = clothing.Categories.Select(c => new Category
            {
               Id = c.CategoryId,
                Name = c.Category.Name,

            }).ToList();



            return CreatedAtAction(nameof(GetProduct), new { id = clothing.Id }, product);

        }

        [Authorize(Policy = "IsAdministrator")]
        [HttpDelete("{id}", Name = "DeleteProduct")]
        public IActionResult Delete(int id)
        {
            Product product = _context.Products.FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                return new BadRequestResult();

            }

            try
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return new BadRequestResult();

            }

            return new OkResult();
        }

        [Authorize(Policy = "IsAdministrator")]
        [HttpPatch("{id}")]
        public ActionResult Update(int id, JsonPatchDocument<Product> patchDoc)
        {
            var product = _context.Products.Include(x => x.Categories).FirstOrDefault(x => x.Id == id);


            if (product == null)
            {
                return NotFound(); // 404 Not Found
            }

            patchDoc.ApplyTo(product);

            _context.SaveChanges();

            return NoContent(); // 204 No Content
        }



    }
}
