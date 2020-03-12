using API_Web_API_Kurs.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API_Web_API_Kurs.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<ProductCategory>()
                .HasKey(sc => new { sc.ProductId, sc.CategoryId });

            modelBuilder.Entity<ProductCategory>()
                .HasOne(a => a.Category)
                .WithMany(m => m.Products)
                .HasForeignKey(a => a.CategoryId);

            modelBuilder.Entity<ProductCategory>()
                .HasOne(m => m.Product)
                .WithMany(a => a.Categories)
                .HasForeignKey(m => m.ProductId);


            Product product1 = new Product
            {
                Id = 1,
                Name = "Goa Brallor",
                UrlSlug = "goa-brallor",
                Price = 1200,
                Description = "Bästa byxorna denna sidan av Båstad",
                ImageUrl =
                    "https://via.placeholder.com/350x400?text=Clothing"};

            Product product2 = new Product
            {
                Id = 2,
                Name = "Små kortbyxor",
                UrlSlug = "små-kortbyxor",
                Price = 1200,
                Description = "Kortaste kortbyxorna",
                ImageUrl =
                    "https://via.placeholder.com/350x400?text=Clothing"
            };

            Product product3 = new Product
            {
                Id = 3,
                Name = "Fina Pjucks",
                UrlSlug = "fina-pjucks",
                Price = 1200,
                Description = "fina skosingar för den medvetne",
                ImageUrl =
                    "https://via.placeholder.com/350x400?text=Clothing"
            };

            Product product4 = new Product
            {
                Id = 4,
                Name = "Blues",
                UrlSlug = "blues",
                Price = 1200,
                Description = "En bluesig blus för den som är nere",
                ImageUrl =
                    "https://via.placeholder.com/350x400?text=Clothing"
            };

            Product product5 = new Product
            {
                Id = 5,
                Name = "Caliente",
                UrlSlug = "caliente",
                Price = 1200,
                Description = "Het tröja",
                ImageUrl =
                    "https://via.placeholder.com/350x400?text=Clothing"
            };


            Product product6 = new Product
            {
                Id = 6,
                Name = "Någon produkt",
                UrlSlug = "någon-produkt",
                Price = 1200,
                Description = "En otrolig produkt på denna sida",
                ImageUrl =
                    "https://via.placeholder.com/350x400?text=Clothing"
            };


            Product product7 = new Product
            {
                Id = 7,
                Name = "Ännu något",
                UrlSlug = "ännu-något",
                Price = 1200,
                Description = "Ännu en grej",
                ImageUrl =
                    "https://via.placeholder.com/350x400?text=Clothing"
            };


            Category category1 = new Category
            {
                Id = 1,
                Name = "Byxor",
                UrlSlug = "byxor",
                ImageUrl = "https://via.placeholder.com/350x400?text=Category",

            };

            Category category2 = new Category
            {
                Id = 2,
                Name = "Herrkläder",
                UrlSlug = "herrkläder",
                ImageUrl = "https://via.placeholder.com/350x400?text=Category"

            };

            Category category3 = new Category
            {
                Id = 3,
                Name = "Damkläder",
                UrlSlug = "damkläder",
                ImageUrl = "https://via.placeholder.com/350x400?text=Category"

            };


            Category category4 = new Category
            {
                Id = 4,
                Name = "Klänningar",
                UrlSlug = "byxor",
                ImageUrl = "https://via.placeholder.com/350x400?text=Category"
            };


            Category category5 = new Category
            {
                Id = 5,
                Name = "Blusar",
                UrlSlug = "blusar",
                ImageUrl = "https://via.placeholder.com/350x400?text=Category"
            };


            Category category6 = new Category
            {
                Id = 6,
                Name = "Tröjor",
                UrlSlug = "tröjor",
                ImageUrl = "https://via.placeholder.com/350x400?text=Category"
            };


            Category category7 = new Category
            {
                Id = 7,
                Name = "Skor",
                UrlSlug = "skor",
                ImageUrl = "https://via.placeholder.com/350x400?text=Category"
            };





            ProductCategory productCategory1 = new ProductCategory
                ();
            productCategory1.Id = 1;
            productCategory1.ProductId = 1;
            productCategory1.CategoryId = 1;

            ProductCategory productCategory2 = new ProductCategory
                ();
            productCategory2.Id = 2;
            productCategory2.ProductId = 1;
            productCategory2.CategoryId = 2;



            ProductCategory productCategory3 = new ProductCategory
                ();
            productCategory3.Id = 3;
            productCategory3.ProductId = 2;
            productCategory3.CategoryId = 1;

            ProductCategory productCategory4 = new ProductCategory
                ();
            productCategory4.Id = 4;
            productCategory4.ProductId = 2;
            productCategory4.CategoryId = 2;



            ProductCategory productCategory5 = new ProductCategory
                ();
            productCategory5.Id = 5;
            productCategory5.ProductId = 3;
            productCategory5.CategoryId = 1;

            ProductCategory productCategory6 = new ProductCategory
                ();
            productCategory6.Id = 6;
            productCategory6.ProductId = 3;
            productCategory6.CategoryId = 2;



            ProductCategory productCategory7 = new ProductCategory
                ();
            productCategory7.Id = 7;
            productCategory7.ProductId = 4;
            productCategory7.CategoryId = 2;

            ProductCategory productCategory8 = new ProductCategory
                ();
            productCategory8.Id = 8;
            productCategory8.ProductId = 4;
            productCategory8.CategoryId = 3;



            ProductCategory productCategory9 = new ProductCategory
                ();
            productCategory9.Id = 9;
            productCategory9.ProductId = 5;
            productCategory9.CategoryId = 6;

            ProductCategory productCategory10 = new ProductCategory
                ();
            productCategory10.Id = 10;
            productCategory10.ProductId = 5;
            productCategory10.CategoryId = 3;



            ProductCategory productCategory11 = new ProductCategory
                ();
            productCategory11.Id = 11;
            productCategory11.ProductId = 6;
            productCategory11.CategoryId = 4;

            ProductCategory productCategory12 = new ProductCategory
                ();
            productCategory12.Id = 12;
            productCategory12.ProductId = 6;
            productCategory12.CategoryId = 3;



            ProductCategory productCategory13 = new ProductCategory
                ();
            productCategory13.Id = 13;
            productCategory13.ProductId = 7;
            productCategory13.CategoryId = 5;

            ProductCategory productCategory14 = new ProductCategory
                ();
            productCategory14.Id = 14;
            productCategory14.ProductId = 7;
            productCategory14.CategoryId = 3;




            Menu menu = new Menu { Id = 1, Name = "MainMenu" };

            MenuItem menuItem1 = new MenuItem { Id = 1, Name = "Byxor", Link = "/byxor", Priority = 1, MenuId = 1 };

            MenuItem menuItem7 = new MenuItem { Id = 2, Name = "Herrkläder", Link = "/herrkläder", Priority = 7, MenuId = 1 };
            MenuItem menuItem6 = new MenuItem { Id = 3, Name = "Damkläder", Link = "/damkläder", Priority = 6, MenuId = 1 };
            MenuItem menuItem2 = new MenuItem { Id = 4, Name = "Klännigar", Link = "/klänningar", Priority = 2, MenuId = 1 };

            MenuItem menuItem3 = new MenuItem { Id = 5, Name = "Blusar", Link = "/blusar", Priority = 3, MenuId = 1 };

            MenuItem menuItem4 = new MenuItem { Id = 6, Name = "Tröjor", Link = "/tröjor", Priority = 4, MenuId = 1 };

            MenuItem menuItem5 = new MenuItem { Id = 7, Name = "Skor", Link = "/skor", Priority = 5, MenuId = 1 };





            modelBuilder.Entity<Product>().HasData(product1);
            modelBuilder.Entity<Product>().HasData(product2);
            modelBuilder.Entity<Product>().HasData(product3);
            modelBuilder.Entity<Product>().HasData(product4);
            modelBuilder.Entity<Product>().HasData(product5);
            modelBuilder.Entity<Product>().HasData(product6);
            modelBuilder.Entity<Product>().HasData(product7);


            modelBuilder.Entity<Category>().HasData(category1);
            modelBuilder.Entity<Category>().HasData(category2);
            modelBuilder.Entity<Category>().HasData(category3);
            modelBuilder.Entity<Category>().HasData(category4);
            modelBuilder.Entity<Category>().HasData(category5);
            modelBuilder.Entity<Category>().HasData(category6);
            modelBuilder.Entity<Category>().HasData(category7);



            modelBuilder.Entity<ProductCategory>().HasData(productCategory1);
            modelBuilder.Entity<ProductCategory>().HasData(productCategory2);
            modelBuilder.Entity<ProductCategory>().HasData(productCategory3);
            modelBuilder.Entity<ProductCategory>().HasData(productCategory4);
            modelBuilder.Entity<ProductCategory>().HasData(productCategory5);
            modelBuilder.Entity<ProductCategory>().HasData(productCategory6);
            modelBuilder.Entity<ProductCategory>().HasData(productCategory7);
            modelBuilder.Entity<ProductCategory>().HasData(productCategory8);
            modelBuilder.Entity<ProductCategory>().HasData(productCategory9);
            modelBuilder.Entity<ProductCategory>().HasData(productCategory10);
            modelBuilder.Entity<ProductCategory>().HasData(productCategory11);
            modelBuilder.Entity<ProductCategory>().HasData(productCategory12);
            modelBuilder.Entity<ProductCategory>().HasData(productCategory13);
            modelBuilder.Entity<ProductCategory>().HasData(productCategory14);


            modelBuilder.Entity<Menu>().HasData(menu);
            modelBuilder.Entity<MenuItem>().HasData(menuItem1);
            modelBuilder.Entity<MenuItem>().HasData(menuItem2);
            modelBuilder.Entity<MenuItem>().HasData(menuItem3);
            modelBuilder.Entity<MenuItem>().HasData(menuItem4);
            modelBuilder.Entity<MenuItem>().HasData(menuItem5);
            modelBuilder.Entity<MenuItem>().HasData(menuItem6);
            modelBuilder.Entity<MenuItem>().HasData(menuItem7);





        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Menu> Menus { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }
    }
}
