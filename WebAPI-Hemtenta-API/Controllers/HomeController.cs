using System.Collections.Generic;
using System.Linq;
using API_Web_API_Kurs.Data;
using API_Web_API_Kurs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Web_API_Kurs.Controllers
{
    [Route("api/home")]
    [ApiController]
    public class SiteController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public SiteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Product
        [HttpGet]
        public JsonResult Get()
        {

            var products  = _context.Products
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
                        categoryId = c.CategoryId,
                        categoryName = c.Category.Name,

                    })

                }).ToList();
            List<Category> categories = _context.Categories.ToList();
            Menu mainMenu = _context.Menus.Include(x => x.MenuItems).
                FirstOrDefault(x => x.Name == "MainMenu");

            var result = new
            {
                Products = products,
                Categories = categories,
                MainMenu = mainMenu,
            };
            return new JsonResult(result);


        }

   
    }
}
