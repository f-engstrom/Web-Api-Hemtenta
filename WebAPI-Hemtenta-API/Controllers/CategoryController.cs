using System;
using System.Linq;
using API_Web_API_Kurs.Data;
using API_Web_API_Kurs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Web_API_Kurs.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }



        // GET: api/Category/5
        [HttpGet("{id}", Name = "GetProductsForCategory")]
        public JsonResult GetProductsForCategory(int id)
        {

            var productsForCategory = _context.ProductCategories
                .Include(x => x.Product)
                .Where(x => x.CategoryId == id)
                .Include(x => x.Category)
                .ToList();



            return new JsonResult(productsForCategory);


        }

        [HttpGet]
        public JsonResult Get()
        {

            var categories = _context.Categories
                .ToList();



            return new JsonResult(categories);


        }


        [HttpPost]
        public IActionResult Post(Category category)
        {


            string urlSulg = category.Name.ToLower().Replace(" ", "-");

            category.UrlSlug = urlSulg;

            try
            {
                _context.Add(category);
                _context.SaveChanges();

            }
            catch (Exception e)
            {
                return BadRequest(e.GetBaseException());
            }



            //return new CreatedResult("https://localhost:44373/api/category", category);
            return CreatedAtAction(nameof(GetProductsForCategory), new { id = category.Id }, category);



        }

        [HttpDelete("{id}", Name="DeleteCategory")]
        public IActionResult Delete(int id)
        {
            Category category = _context.Categories.FirstOrDefault(x => x.Id == id);

            if (category == null)
            {
                return new BadRequestResult();

            }

            try
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return new BadRequestResult();

            }

            return new OkResult();
        }


    }
}
