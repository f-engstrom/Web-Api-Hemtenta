using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using WebAPI_Hemtenta.Models;
using static System.Console;
using static WebAPI_Hemtenta.Products.HelperMethods;
using static WebAPI_Hemtenta.Program;

namespace WebAPI_Hemtenta.Products
{
    static partial class ProductAdminMethods
    {
        public static bool EditProduct(Product product)
        {
            var categories = _a.GetResourceAsync<List<Category>>(Api.CategoryApi).Result;
            bool edited = false;
            string uriString = $"{Api.ProductApi}/{product.Id}";
            Uri patchApiForProduct = new Uri(uriString);

            Clear();
            WriteLine(
                "Press Esc to when done editing. To add or remove a category add the id.".PadRight(Program.WindowWidth,
                    '#'));
            WriteLine("".PadRight(Program.WindowWidth, '#'));


            Coordinates productPropertiesCoordinates =
                new Coordinates(ContentCursorPosLeft - 3, ContentCursorPosTop);

            int propertyAndValueSpacing = 15;
            product.PrintPropertiesWithValues(productPropertiesCoordinates, new PrintSettings
            {
                ChosenListProperty = "Id",
                PrintFlatList = true,
                PropertiesAndValuesSpacing = propertyAndValueSpacing,
                PropertiesSpacing = 2,
            });

            OptionsPrinter("        ");


            PrintCategories(categories);


            List<string> propertyNames = productPropertiesCoordinates.SavedCoordinates.Keys.ToList();
            int moveCursor = 1;
            bool shouldNotExit = true;
            ConsoleKeyInfo consoleKeyInfo;
            Dictionary<string, string> changes = new Dictionary<string, string>();
            List<int> oldProductCategoryIds = product.Categories.Select(c => c.Id).ToList();
            List<int> newProductCategoryIds = oldProductCategoryIds;
            bool correctKey;


            while (shouldNotExit)
            {
                SetCursorPosition(
                    productPropertiesCoordinates.SavedCoordinates[propertyNames[moveCursor]].X + propertyAndValueSpacing,
                    productPropertiesCoordinates.SavedCoordinates[propertyNames[moveCursor]].Y);

                do
                {
                    consoleKeyInfo = ReadKey(true);

                    correctKey = !(consoleKeyInfo.Key == ConsoleKey.UpArrow ||
                                   consoleKeyInfo.Key == ConsoleKey.DownArrow ||
                                   consoleKeyInfo.Key == ConsoleKey.Escape || consoleKeyInfo.Key == ConsoleKey.Enter);
                } while (correctKey);


                switch (consoleKeyInfo.Key)
                {
                    case ConsoleKey.UpArrow:

                        if (moveCursor > 1)
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

                        if (!currentProperty.ToUpper().Contains("ID"))
                        {
                            if (currentProperty.ToUpper().Contains("CATEGORIES"))
                            {
                                EraseOldText(35);

                                int categoryId = Convert.ToInt32(ReadLine());

                                if (categories.Any(ca => ca.Id == categoryId))
                                {
                                    if (newProductCategoryIds == oldProductCategoryIds)
                                    {
                                        newProductCategoryIds = product.Categories.Select(c => c.Id).ToList();
                                    }

                                    if (newProductCategoryIds.Contains(categoryId))
                                    {
                                        SetCursorPosition(
                                            productPropertiesCoordinates.SavedCoordinates["Categories"].X +
                                            propertyAndValueSpacing,
                                            productPropertiesCoordinates.SavedCoordinates["Categories"].Y);
                                        EraseOldText(35);
                                        Write("Category removed.");
                                        newProductCategoryIds.Remove(categoryId);
                                    }
                                    else
                                    {
                                        SetCursorPosition(
                                            productPropertiesCoordinates.SavedCoordinates["Categories"].X +
                                            propertyAndValueSpacing,
                                            productPropertiesCoordinates.SavedCoordinates["Categories"].Y);
                                        EraseOldText(35);
                                        Write("Category sucessfully added added.");
                                        newProductCategoryIds.Add(categoryId);
                                    }

                                    string productCategoryIdsString = string.Join(", ", newProductCategoryIds);

                                    SetCursorPosition(
                                        productPropertiesCoordinates.SavedCoordinates["Categories"].X +
                                        propertyAndValueSpacing,
                                        productPropertiesCoordinates.SavedCoordinates["Categories"].Y);
                                    EraseOldText(35);
                                    WriteLine(productCategoryIdsString);
                                }
                                else
                                {
                                    SetCursorPosition(
                                        productPropertiesCoordinates.SavedCoordinates["Categories"].X +
                                        propertyAndValueSpacing,
                                        productPropertiesCoordinates.SavedCoordinates["Categories"].Y);
                                    EraseOldText(35);
                                    Write("Invalid category Id.");
                                    Thread.Sleep(1000);
                                }
                            }
                            else
                            {
                                EraseOldText(35);

                                string input = ReadLine();

                                bool added = changes.TryAdd(currentProperty, input);

                                if (!added)
                                {
                                    changes[currentProperty] = input;
                                }


                            }
                        }


                        break;

                    case ConsoleKey.Escape:
                        shouldNotExit = false;

                        break;
                }


                SetCursorPosition(
                    productPropertiesCoordinates.SavedCoordinates[propertyNames[moveCursor]].X + propertyAndValueSpacing,
                    productPropertiesCoordinates.SavedCoordinates[propertyNames[moveCursor]].Y);
            }


            bool isUnchanged = changes.Count <= 0 && newProductCategoryIds == oldProductCategoryIds;

            if (isUnchanged) return edited;

            bool b;

            JsonPatchDocument jsonPatchDocument = new JsonPatchDocument();

            OptionsPrinter("Apply edits? (Y)es (N)o");
            do
            {
                consoleKeyInfo = ReadKey(true);

                b = !(consoleKeyInfo.Key == ConsoleKey.Y || consoleKeyInfo.Key == ConsoleKey.N);
            } while (b);

            switch (consoleKeyInfo.Key)
            {
                case ConsoleKey.Y:
                    {
                        foreach (var change in changes)
                        {
                            bool canConvert = decimal.TryParse(change.Value, out var price);

                            if (canConvert)
                            {
                                jsonPatchDocument.Replace(change.Key, price);
                            }
                            else
                            {
                                jsonPatchDocument.Replace(change.Key, change.Value);

                                if (change.Key.ToUpper().Contains("NAME"))
                                {
                                    string urlSulg = change.Value.ToLower().Replace(" ", "-");


                                    jsonPatchDocument.Replace("UrlSlug", urlSulg);
                                }
                            }
                        }

                        foreach (var categoryId in oldProductCategoryIds)
                        {
                            if (!newProductCategoryIds.Contains(categoryId))
                            {
                                jsonPatchDocument.Remove($"Categories/{oldProductCategoryIds.IndexOf(categoryId)}");
                            }
                        }

                        foreach (var categoryId in newProductCategoryIds)
                        {
                            if (!oldProductCategoryIds.Contains(categoryId))
                            {
                                ProductCategory productCategory = new ProductCategory();

                                productCategory.CategoryId = categoryId;

                                jsonPatchDocument.Add($"Categories/-", productCategory);
                            }
                        }


                        var stringContent = new StringContent(JsonConvert.SerializeObject(jsonPatchDocument), Encoding.UTF8,
                            "application/json");

                        var response = _a.PatchResourceAsync(patchApiForProduct, stringContent).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            Clear();
                            SetCursorPosition(MenuCursorPosLeft, MenuCursorPosTop);
                            WriteLine("Changes Sucessfully applied");
                            Thread.Sleep(1500);
                            edited = true;
                        }
                        else
                        {
                            Clear();
                            SetCursorPosition(MenuCursorPosLeft, MenuCursorPosTop);
                            WriteLine("Something went wrong with applying the changes.");
                            Thread.Sleep(1500);
                        }

                        break;
                    }
                case ConsoleKey.N:
                    Clear();
                    SetCursorPosition(MenuCursorPosLeft, MenuCursorPosTop);
                    WriteLine("No changes applied");
                    Thread.Sleep(1500);
                    break;
            }


            return edited;

        }

        private static void PrintCategories(List<Category> categories)
        {
            int x = ContentCursorPosLeft + 60;
            int y = ContentCursorPosTop - 2;


            SetCursorPosition(x, y);
            WriteLine("Available Categories");
            SetCursorPosition(x, y + 1);
            WriteLine("___________________________________");

            SetCursorPosition(x, y += 3);
            foreach (var category in categories)
            {
                if (x > 90)
                {
                    y++;
                    x = 70;
                }

                SetCursorPosition(x, y);
                string categoryString = $"{category.Id}. {category.Name}";
                WriteLine(categoryString);
                x += categoryString.Length + 2;
            }
        }


    }
}