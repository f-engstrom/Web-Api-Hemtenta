using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Converters;
using Newtonsoft.Json;
using static System.Console;
using static WebAPI_Hemtenta.Program;

namespace WebAPI_Hemtenta
{
    class ProductAdminMethods
    {
        static API a = new API();

        static public void ListProducts()
        {

            Clear();
            bool shouldNotExit = true;

            while (shouldNotExit)
            {

                List<Product> products = a.GetResourceAsync<Product>(API.ProductAPI).Result;

                foreach (var product in products)
                {

                    WriteLine($"Id {product.Id} | Name {product.Name}");

                }


                int cursorLeft = CursorLeft;
                int cursorTop = CursorTop;
                WriteLine("(V)iew (D)elete (E)dit");

                ConsoleKeyInfo keyPressed = ReadKey(true);

                SetCursorPosition(cursorLeft, cursorTop);
                WriteLine(" ".PadRight("(V)iew (D)elete (E)dit".Length));
                SetCursorPosition(cursorLeft, cursorTop);
                WriteLine("ID: ");
                SetCursorPosition(cursorLeft + 4, cursorTop);
                int id = Convert.ToInt32(ReadLine());

                Product chosenProduct = products.FirstOrDefault(x => x.Id == id);

                switch (keyPressed.Key)
                {

                    case ConsoleKey.V:

                        Clear();

                        ListProduct(chosenProduct);


                        break;

                    case ConsoleKey.D:

                        Clear();

                        DeleteProduct(chosenProduct);

                        break;


                    case ConsoleKey.E:

                        Clear();

                        EditProduct(chosenProduct);

                        break;

                    case ConsoleKey.Escape:

                        Clear();

                        shouldNotExit = false;

                        break;



                }

            }






        }

        public static void AddProduct()
        {
            Uri categoryAPI = new Uri("https://localhost:44373/api/category");
            List<Category> categories = a.GetResourceAsync<Category>(categoryAPI).Result;
            Clear();

            Uri productAPI = new Uri("https://localhost:44373/api/product");
            ProductDTO product = new ProductDTO();


            product = CreateProductFromInput();

            product.CategoriesId = AddCategoriesToProduct(categories);


            try
            {
                var response = a.PostResourceAsync(productAPI, product).Result;
                Clear();
                WriteLine($"Poduct added sucessfully. {response}");
                Thread.Sleep(2000);

            }
            catch (Exception e)
            {
                Clear();
                WriteLine("Something went wrong with adding the product.");
                Thread.Sleep(2000);

            }


        }


        private static void DeleteProduct(Product product)
        {

            Clear();
            bool b;

            Coordinates coordinates = new Coordinates(MenuCursorPosLeft, MenuCursorPosTop);
            product.PrintPropertiesWithValues(coordinates);

            WriteLine("Are you sure you want to delete this product? (Y)es or (N)o");

            ConsoleKeyInfo consoleKeyInfo;

            do
            {
                consoleKeyInfo = ReadKey(true);

                b = !(consoleKeyInfo.Key == ConsoleKey.Y || consoleKeyInfo.Key == ConsoleKey.N);
            } while (b);

            if (consoleKeyInfo.Key == ConsoleKey.Y)
            {

                try
                {
                    var response = a.DeleteResourceAsync(API.ProductAPI, product.Id).Result;
                    Clear();
                    WriteLine($"Poduct sucessfully deleted. {response}");
                    Thread.Sleep(2000);

                }
                catch (Exception e)
                {
                    Clear();
                    WriteLine($"Something went wrong with deleting the category.");
                    Thread.Sleep(2000);

                }

            }



        }

        private static ProductDTO CreateProductFromInput()
        {
            bool customerExists = false;
            bool b = true;
            bool doNotExitLoop = true;


            ProductDTO product = new ProductDTO();

            int x = 10;
            int y = 10;

            string nameInput = "Name: ";
            string descriptionInput = "Description: ";
            string priceInput = "Price: ";
            string imageUrlInput = "ImageUrl: ";

            do
            {
                SetCursorPosition(x, y);
                WriteLine(nameInput);
                SetCursorPosition(x, y + 2);
                WriteLine(descriptionInput);
                SetCursorPosition(x, y + 4);
                WriteLine(priceInput);
                SetCursorPosition(x, y + 6);
                WriteLine(imageUrlInput);



                SetCursorPosition(x + nameInput.Length, y);
                string productName = ReadLine();
                SetCursorPosition(x + descriptionInput.Length, y + 2);
                string productDescription = ReadLine();
                SetCursorPosition(x + priceInput.Length, y + 4);
                decimal productPrice = Convert.ToDecimal(ReadLine());
                SetCursorPosition(x + imageUrlInput.Length, y + 6);
                string productImageUrl = ReadLine();

                SetCursorPosition(x, y + 9);
                WriteLine("Is this correct? (Y)es or (N)o");

                ConsoleKeyInfo consoleKeyInfo;

                do
                {
                    consoleKeyInfo = ReadKey(true);

                    b = !(consoleKeyInfo.Key == ConsoleKey.Y || consoleKeyInfo.Key == ConsoleKey.N);
                } while (b);


                if (consoleKeyInfo.Key == ConsoleKey.Y)
                {

                    product.Name = productName;
                    product.Description = productDescription;
                    product.Price = productPrice;
                    product.ImageUrl = productImageUrl;
                    doNotExitLoop = false;


                }

                if (consoleKeyInfo.Key == ConsoleKey.N)
                {
                    Clear();

                }

            } while (doNotExitLoop);

            return product;
        }

        private static int[] AddCategoriesToProduct(List<Category> categories)
        {

            int x = 60;
            int y = 10;

            bool b = true;
            ConsoleKeyInfo consoleKeyInfo;





            SetCursorPosition(x, y);
            WriteLine("Available Categories");
            SetCursorPosition(x, y + 1);
            WriteLine("___________________________________");

            int indexer = 1;
            SetCursorPosition(x, y += 3);
            foreach (var category in categories)
            {
                if (x > 90)
                {
                    y++;
                    x = 60;
                }
                SetCursorPosition(x, y);
                string categoryString = $"{indexer++}. {category.Name}";
                WriteLine(categoryString);
                x += categoryString.Length + 2;

            }

            bool doNotExitLoop = true;

            List<Category> addedCategories = new List<Category>();
            int cursorLeft = CursorLeft;
            int cursorTop = CursorTop;
            int inputCursorPosleft = 10;
            int inputCUrsorPosTop = 19;
            int categoriesCursorPosLeft = inputCursorPosleft;
            SetCursorPosition(inputCursorPosleft, inputCUrsorPosTop);
            WriteLine("                                                                              ");

            string categoriesAdded = "Categories added: ";

            SetCursorPosition(inputCursorPosleft, inputCUrsorPosTop - 1);
            WriteLine(categoriesAdded);
            do
            {
                SetCursorPosition(inputCursorPosleft, inputCUrsorPosTop);
                WriteLine("Add a category by number: ");
                SetCursorPosition("Add a category by number: ".Length + inputCursorPosleft + 1, inputCUrsorPosTop);
                int categoryNr = Convert.ToInt32(ReadLine());
                SetCursorPosition("Add a category by number: ".Length + inputCursorPosleft + 1, inputCUrsorPosTop);
                WriteLine("   ");
                addedCategories.Add(categories[categoryNr - 1]);
                categoriesAdded += categories[categoryNr - 1].Name + ", ";
                SetCursorPosition(inputCursorPosleft, inputCUrsorPosTop - 1);
                WriteLine(categoriesAdded);




                SetCursorPosition(inputCursorPosleft, inputCUrsorPosTop + 2);
                WriteLine("Add additional category? (Y)es (N)o");
                SetCursorPosition(inputCursorPosleft, inputCUrsorPosTop + 2);
                do
                {
                    consoleKeyInfo = ReadKey(true);

                    b = !(consoleKeyInfo.Key == ConsoleKey.Y || consoleKeyInfo.Key == ConsoleKey.N);

                } while (b);
                SetCursorPosition(inputCursorPosleft, inputCUrsorPosTop + 2);
                WriteLine("                                                 ");


                if (consoleKeyInfo.Key == ConsoleKey.N)
                {
                    doNotExitLoop = false;

                }

            } while (doNotExitLoop);



            return addedCategories.Select(c => c.Id).ToArray();
        }



        public static void ListProduct(Product product)
        {

            Clear();

            Coordinates coordinates = new Coordinates(MenuCursorPosLeft, MenuCursorPosTop);
            product.PrintPropertiesWithValues(coordinates);
            int cursorTop = CursorTop;


            bool shouldNotExit = true;

            while (shouldNotExit)
            {
                SetCursorPosition(coordinates.X, cursorTop + 1);
                WriteLine("(D)elete (E)dit");

                ConsoleKeyInfo keyPressed = ReadKey(true);


                switch (keyPressed.Key)
                {

                    case ConsoleKey.D:

                        Clear();

                        DeleteProduct(product);

                        break;


                    case ConsoleKey.E:

                        Clear();

                        EditProduct(product);

                        shouldNotExit = false;

                        break;

                    case ConsoleKey.Escape:

                        Clear();

                        shouldNotExit = false;

                        break;



                }

            }



            ListProducts();

        }

        private static void EditProduct(Product product)
        {

            string uriString = $"{API.ProductAPI}/{product.Id}";
            Uri patchAPIForProduct = new Uri(uriString);

            Coordinates productPropertiesCoordinates = new Coordinates(MenuCursorPosLeft, MenuCursorPosTop);

            product.PrintPropertiesWithValues(productPropertiesCoordinates);



            void EraseOldText(int length)
            {
                int cursorLeft = CursorLeft;
                int cursorTop = CursorTop;
                Write(" ".PadRight(length));
                SetCursorPosition(cursorLeft, cursorTop);
            }

            //int[,] cursorPositions = new int[2, 2]
            //{
            //    {MenuCursorPosLeft, MenuCursorPosTop },
            //    {MenuCursorPosLeft, MenuCursorPosTop + 1}

            //};
            //string forstaRutanText = "Första rutan: ";
            //string andraRutanText = "Andra rutan: ";
            //string inputEtt = "";
            //string inputTvå = "";

            List<string> propertyNames = productPropertiesCoordinates.SavedCoordinates.Keys.ToList();


            //SetCursorPosition(cursorPositions[0, 0], cursorPositions[0, 1]);
            //WriteLine(forstaRutanText);
            //SetCursorPosition(cursorPositions[1, 0], cursorPositions[1, 1]);
            //WriteLine(andraRutanText);
            //SetCursorPosition(MenuCursorPosLeft + 30, MenuCursorPosTop);
            //WriteLine(" ".PadRight(inputEtt.Length));
            //SetCursorPosition(MenuCursorPosLeft + 30, MenuCursorPosTop);
            //WriteLine($"Input ett: {inputEtt}");
            //SetCursorPosition(MenuCursorPosLeft + 30, MenuCursorPosTop + 1);
            //WriteLine(" ".PadRight(inputTvå.Length));
            //SetCursorPosition(MenuCursorPosLeft + 30, MenuCursorPosTop + 1);
            //WriteLine($"Input två: {inputTvå}");

            //SetCursorPosition(forstaRutanText.Length + cursorPositions[0, 0], cursorPositions[0, 1]);

            int moveCursor = 0;

            bool shouldNotExit = true;


            //int oldLengthInputEtt = 0;
            //int oldLengthInputTvå = 0;

            ConsoleKeyInfo consoleKeyInfo;

            while (shouldNotExit)
            {


                consoleKeyInfo = ReadKey(true);

                //if (oldLengthInputEtt > 0 | oldLengthInputTvå > 0)
                //{
                //    SetCursorPosition(MenuCursorPosLeft + 30, MenuCursorPosTop);
                //    WriteLine(" ".PadRight(oldLengthInputEtt));

                //    SetCursorPosition(MenuCursorPosLeft + 30, MenuCursorPosTop + 1);
                //    WriteLine(" ".PadRight(oldLengthInputTvå));

                //}

                //SetCursorPosition(MenuCursorPosLeft + 30, MenuCursorPosTop);
                //WriteLine($"Input ett: {inputEtt}");
                //SetCursorPosition(MenuCursorPosLeft + 30, MenuCursorPosTop + 1);
                //WriteLine($"Input två: {inputTvå}");



                SetCursorPosition(productPropertiesCoordinates.SavedCoordinates[propertyNames[moveCursor]].X + 20, productPropertiesCoordinates.SavedCoordinates[propertyNames[moveCursor]].Y);
                switch (consoleKeyInfo.Key)
                {

                    case ConsoleKey.UpArrow:

                        if (moveCursor > 0)
                        {
                            moveCursor--;
                        }

                        break;

                    case ConsoleKey.DownArrow:

                        if (moveCursor < propertyNames.Count - 1)
                        {
                            moveCursor++;
                        }

                        break;

                    case ConsoleKey.Enter:


                        if (!propertyNames[moveCursor].ToUpper().Contains("ID"))
                        {

                            WriteLine("InputField");


                        }

                        //if (moveCursor == 0)
                        //{
                        //    if (inputEtt.Length > 0)
                        //    {
                        //        oldLengthInputEtt = inputEtt.Length;
                        //        EraseOldText(oldLengthInputEtt);

                        //    }

                        //    inputEtt = ReadLine() ?? inputEtt;

                        //}
                        //if (moveCursor == 1)
                        //{
                        //    if (inputTvå.Length > 0)
                        //    {
                        //        oldLengthInputTvå = inputTvå.Length;
                        //        EraseOldText(oldLengthInputTvå);

                        //    }
                        //    inputTvå = ReadLine() ?? inputTvå;

                        //}

                        break;

                    case ConsoleKey.Escape:
                        shouldNotExit = false;

                        break;

                }


                SetCursorPosition(productPropertiesCoordinates.SavedCoordinates[propertyNames[moveCursor]].X + 20, productPropertiesCoordinates.SavedCoordinates[propertyNames[moveCursor]].Y);





            }





            //bool b;
            //ConsoleKeyInfo consoleKeyInfo;
            //JsonPatchDocument jsonPatchDocument = new JsonPatchDocument();
            //SetCursorPosition(coordinatesAfterProduct.X, coordinatesAfterProduct.X + 2);
            //WriteLine("Apply edits? (Y)es (N)o");
            //SetCursorPosition(coordinatesAfterProduct.X, coordinatesAfterProduct.X + 2);
            //do
            //{
            //    consoleKeyInfo = ReadKey(true);

            //    b = !(consoleKeyInfo.Key == ConsoleKey.Y || consoleKeyInfo.Key == ConsoleKey.N);

            //} while (b);

            //if (consoleKeyInfo.Key == ConsoleKey.Y)
            //{


            //   // string urlSulg = productDto.Name.ToLower().Replace(" ", "-");


            //    jsonPatchDocument.Replace("Name", "testPatch");


            //    var stringContent = new StringContent(JsonConvert.SerializeObject(jsonPatchDocument), Encoding.UTF8, "application/json");

            //    var response = a.PatchResourceAsync(patchAPIForProduct, stringContent).Result;



            //}

            //if (consoleKeyInfo.Key == ConsoleKey.N)
            //{
            //    Clear();
            //    SetCursorPosition(coordinates.X, coordinates.Y);
            //    Console.WriteLine("No changes applied");
            //    Thread.Sleep(1500);
            //}








        }
    }
}