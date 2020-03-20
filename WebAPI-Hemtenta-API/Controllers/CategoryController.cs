using System;
using System.Linq;
using API_Web_API_Kurs.Data;
using API_Web_API_Kurs.Models;
using API_Web_API_Kurs.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace API_Web_API_Kurs.Controllers
{
    [Authorize]
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
        public IActionResult GetCategory(int id)
        {

            Category category = _context.Categories.FirstOrDefault(x => x.Id == id);

            if (category == null)
            {
                return new BadRequestResult();

            }


            return new JsonResult(category);


        }

        [HttpGet]
        public IActionResult Get()
        {

            var categories = _context.Categories
                .ToList();

            if (categories == null)
            {
                return new BadRequestResult();

            }


            return new JsonResult(categories);


        }

        [Authorize(Policy = "IsAdministrator")]
        [HttpPost]
        public IActionResult CreateCategory(CategoryDto categoryDto)
        {
            Category category = new Category();
            category.Name = categoryDto.Name;
            category.UrlSlug = categoryDto.Name.ToLower().Replace(" ", "-");
            category.ImageUrl = categoryDto.ImageUrl;


            try
            {
                _context.Add(category);
                _context.SaveChanges();

            }
            catch (Exception e)
            {
                return BadRequest(e.GetBaseException());
            }


            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);



        }

        [Authorize(Policy = "IsAdministrator")]
        [HttpDelete("{id}", Name = "DeleteCategory")]
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

        [Authorize(Policy = "IsAdministrator")]
        [HttpPatch("{id}")]
        public ActionResult Update(int id, JsonPatchDocument<Category> patchDoc)
        {
            var category = _context.Categories.FirstOrDefault(x => x.Id == id);


            if (category == null)
            {
                return NotFound(); // 404 Not Found
            }

            patchDoc.ApplyTo(category);

            _context.SaveChanges();

            return NoContent(); // 204 No Content
        }


    }
}
