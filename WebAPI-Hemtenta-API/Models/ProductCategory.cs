﻿namespace API_Web_API_Kurs.Models
{
    public class ProductCategory
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }

        public Product Product { get; set; }

        public Category Category { get; set; }


        public ProductCategory()
        {
            
        }


        public ProductCategory(Product product)
        {
            Product = product;
        }

    }
}