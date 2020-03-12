using System.ComponentModel.DataAnnotations;

namespace WebAPI_Hemtenta
{
    public class ProductDTO
    {

        //public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }
        public string UrlSlug { get; set; }

        public int[] CategoriesId { get; set; }
       

        [Display(Name = "Climate Compensated")]
        public bool ClimateCompensated { get; set; }

       

    }
}
