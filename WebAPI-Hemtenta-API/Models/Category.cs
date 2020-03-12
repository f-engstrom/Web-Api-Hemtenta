using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API_Web_API_Kurs.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "Image URL")]

        public string ImageUrl { get; set; }

        public string UrlSlug { get; set; }

        [JsonIgnore]
        public List<ProductCategory> Products { get; set; } = new List<ProductCategory>();
    }
}
