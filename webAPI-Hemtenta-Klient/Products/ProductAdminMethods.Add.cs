using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Unicode;
using System.Threading;
using System.Xml;
using Newtonsoft.Json;
using WebAPI_Hemtenta.Models;
using static System.Console;
using static WebAPI_Hemtenta.Program;
using static WebAPI_Hemtenta.Products.HelperMethods;

namespace WebAPI_Hemtenta.Products
{
    static partial class ProductAdminMethods
    {
        public static void AddProduct()
        {
            Clear();
            WriteLine("Press Esc to when done editing. To add or remove a category add the id.".PadRight(Program.WindowWidth, '#'));
            WriteLine("".PadRight(Program.WindowWidth, '#'));

            OptionsPrinter("    ");

            var categories = _a.GetResourceAsync<List<Category>>(Api.CategoryApi).Result;

            CreateProductDto product = new CreateProductDto();
            Coordinates productPropertiesCoordinates = new Coordinates(ContentCursorPosLeft, ContentCursorPosTop);

            int popertyAndValueSpacing = 15;

            product.PrintProperties(productPropertiesCoordinates, new PrintSettings
            {
                ChosenListProperty = "Id",
                PrintFlatList = true,
                PropertiesAndValuesSpacing = popertyAndValueSpacing,
                PropertiesSpacing = 2,


            });



            PrintCategories(categories);


            List<string> propertyNames = productPropertiesCoordinates.SavedCoordinates.Keys.ToList();
            List<int> productCategoryIds = new List<int>();
            int moveCursor = 0;
            int oldTextLength = 44;
            bool shouldNotExit = true;
            ConsoleKeyInfo consoleKeyInfo;
            bool b;
            bool correctKey;
            bool edited = false;

            while (shouldNotExit)
            {





                SetCursorPosition(productPropertiesCoordinates.SavedCoordinates[propertyNames[moveCursor]].X + popertyAndValueSpacing, productPropertiesCoordinates.SavedCoordinates[propertyNames[moveCursor]].Y);

                do
                {
                    consoleKeyInfo = ReadKey(true);

                    correctKey = !(consoleKeyInfo.Key == ConsoleKey.UpArrow || consoleKeyInfo.Key == ConsoleKey.DownArrow || consoleKeyInfo.Key == ConsoleKey.Escape || consoleKeyInfo.Key == ConsoleKey.Enter);

                } while (correctKey);



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

                        string currentProperty = propertyNames[moveCursor];
                        edited =true;


                        if (currentProperty.ToUpper().Contains("CATEGORIESID"))
                        {
                            EraseOldText(oldTextLength);


                            int categoryId = Convert.ToInt32(ReadLine());

                            if (categories.Any(ca => ca.Id == categoryId))
                            {

                                if (productCategoryIds.Contains(categoryId))
                                {
                                    SetCursorPosition(
                                        productPropertiesCoordinates.SavedCoordinates["CategoriesId"].X +
                                        popertyAndValueSpacing,
                                        productPropertiesCoordinates.SavedCoordinates["CategoriesId"].Y);
                                    EraseOldText(oldTextLength);
                                    Write("Category removed.");
                                    Thread.Sleep(1500);
                                    productCategoryIds.Remove(categoryId);


                                }
                                else
                                {
                                    SetCursorPosition(
                                        productPropertiesCoordinates.SavedCoordinates["CategoriesId"].X +
                                        popertyAndValueSpacing,
                                        productPropertiesCoordinates.SavedCoordinates["CategoriesId"].Y);
                                    EraseOldText(oldTextLength);
                                    Write("Category sucessfully added added.");
                                    Thread.Sleep(1000);
                                    productCategoryIds.Add(categoryId);
                                }

                                string productCategoryIdsString = string.Join(", ", productCategoryIds);

                                SetCursorPosition(
                                    productPropertiesCoordinates.SavedCoordinates["CategoriesId"].X +
                                    popertyAndValueSpacing,
                                    productPropertiesCoordinates.SavedCoordinates["CategoriesId"].Y);
                                EraseOldText(oldTextLength);
                                WriteLine(productCategoryIdsString);

                            }
                            else
                            {
                                SetCursorPosition(
                                    productPropertiesCoordinates.SavedCoordinates["CategoriesId"].X +
                                    popertyAndValueSpacing,
                                    productPropertiesCoordinates.SavedCoordinates["CategoriesId"].Y);
                                EraseOldText(oldTextLength);
                                Write("Invalid category Id.");
                                Thread.Sleep(1000);

                            }

                        }
                        else if (currentProperty.ToUpper().Contains("PRICE"))
                        {
                            EraseOldText(oldTextLength);

                            bool isDecimal = decimal.TryParse(ReadLine(), out decimal price);

                            if (isDecimal)
                            {
                                product.Price = price;

                            }
                            else
                            {
                                SetCursorPosition(
                                    productPropertiesCoordinates.SavedCoordinates["Price"].X +
                                    popertyAndValueSpacing,
                                    productPropertiesCoordinates.SavedCoordinates["Price"].Y);
                                EraseOldText(oldTextLength);
                                WriteLine("Please supply a decimal value for the price.");
                            }

                        }
                        else
                        {


                            EraseOldText(oldTextLength);

                            string input = ReadLine();


                            foreach (var property in product.GetType().GetProperties())
                            {
                                if (property.Name == currentProperty)
                                {

                                    property.SetValue(product, input);
                                }
                            }

                        }




                        break;

                    case ConsoleKey.Escape:
                        shouldNotExit = false;


                        if (productCategoryIds.Count == 0 && edited)
                        {

                            OptionsPrinter("Are you sure you want to exit without adding categories? (Y)es (N)o");
                            do
                            {
                                consoleKeyInfo = ReadKey(true);

                                b = !(consoleKeyInfo.Key == ConsoleKey.Y || consoleKeyInfo.Key == ConsoleKey.N);

                            } while (b);

                            if (consoleKeyInfo.Key == ConsoleKey.N)
                            {
                                shouldNotExit = true;
                            }
                        }


                        break;

                }


                SetCursorPosition(productPropertiesCoordinates.SavedCoordinates[propertyNames[moveCursor]].X + popertyAndValueSpacing, productPropertiesCoordinates.SavedCoordinates[propertyNames[moveCursor]].Y);





            }


            if (product.ImageUrl == null || product.Name == null || product.Price == null ||
                product.Description == null)
            {
                Clear();
                SetCursorPosition(MenuCursorPosLeft, MenuCursorPosTop);
                WriteLine("No changes applied");
                Thread.Sleep(1500);
                return;

            }


            OptionsPrinter("Save product ? (Y)es (N)o");
            do
            {
                consoleKeyInfo = ReadKey(true);

                b = !(consoleKeyInfo.Key == ConsoleKey.Y || consoleKeyInfo.Key == ConsoleKey.N);

            } while (b);

            switch (consoleKeyInfo.Key)
            {
                case ConsoleKey.Y:
                    Clear();

                    product.CategoriesId = productCategoryIds.ToArray();

                    var response = _a.PostResourceAsync(Api.ProductApi, product).Result;

                    if (response.IsSuccessStatusCode)
                    {

                        Clear();
                        SetCursorPosition(MenuCursorPosLeft, MenuCursorPosTop);
                        WriteLine($"Poduct added sucessfully.");
                        Thread.Sleep(2000);

                    }
                    else
                    {

                        Clear();
                        SetCursorPosition(MenuCursorPosLeft, MenuCursorPosTop);
                        WriteLine("Something went wrong with adding the product.");
                        Thread.Sleep(2000);
                    }

                    break;

                case ConsoleKey.N:
                    Clear();
                    SetCursorPosition(MenuCursorPosLeft, MenuCursorPosTop);
                    WriteLine("No changes applied");
                    Thread.Sleep(1500);

                    break;
               
            }
        }

      
    }

   
}