using System.Collections.Generic;
using System.ComponentModel;

namespace WebAPI_Hemtenta.Models
{
    internal class Product
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        [Description("Image URL")]
        public string ImageUrl { get; set; }
        [Description("URL slug")]
        public string  UrlSlug { get; set; }

        public Product()
        {
            
        }


        public List<Category> Categories { get; set; }=  new List<Category>();




    }
}