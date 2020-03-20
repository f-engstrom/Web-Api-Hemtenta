using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

namespace WebAPI_Hemtenta.Categories
{
    partial class CategoryAdminMethods
    {
        private static bool EditCategory(Category category)
        {

            bool edited = false;

            string uriString = $"{Api.CategoryApi}/{category.Id}";
            Uri patchApiForCategory = new Uri(uriString);
            Coordinates categoryPropertiesCoordinates =
                new Coordinates(ContentCursorPosLeft, ContentCursorPosTop);



            Clear();
            WriteLine(
                "Press Esc to when done editing. To add or remove a category add the id.".PadRight(Program.WindowWidth,
                    '#'));
            WriteLine("".PadRight(Program.WindowWidth, '#'));

            category.PrintPropertiesWithValues(categoryPropertiesCoordinates);
            
            OptionsPrinter("        ");

            List<string> propertyNames = categoryPropertiesCoordinates.SavedCoordinates.Keys.ToList();


            int moveCursor = 1;


            ConsoleKeyInfo consoleKeyInfo;

            
            Dictionary<string, string> changes = new Dictionary<string, string>();


            bool correctKey;
            bool shouldNotExit = true;
            while (shouldNotExit)
            {
                


                SetCursorPosition(
                    categoryPropertiesCoordinates.SavedCoordinates[propertyNames[moveCursor]].X + 20,
                    categoryPropertiesCoordinates.SavedCoordinates[propertyNames[moveCursor]].Y);

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


                        if (!currentProperty.ToUpper().Contains("ID"))
                        {
                            EraseOldText(100);

                            string input = ReadLine();

                            bool added = changes.TryAdd(currentProperty, input);

                            if (!added)
                            {
                                changes[currentProperty] = input;
                            }

                        }


                        break;

                    case ConsoleKey.Escape:
                        shouldNotExit = false;

                        break;
                }


                SetCursorPosition(
                    categoryPropertiesCoordinates.SavedCoordinates[propertyNames[moveCursor]].X + 20,
                    categoryPropertiesCoordinates.SavedCoordinates[propertyNames[moveCursor]].Y);
            }



            if (changes.Count > 0)
            {
                bool b;

                JsonPatchDocument jsonPatchDocument = new JsonPatchDocument();
                OptionsPrinter("Apply edits? (Y)es (N)o");
                do
                {
                    consoleKeyInfo = ReadKey(true);

                    b = !(consoleKeyInfo.Key == ConsoleKey.Y || consoleKeyInfo.Key == ConsoleKey.N);
                } while (b);

                if (consoleKeyInfo.Key == ConsoleKey.Y)
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


                    var stringContent = new StringContent(JsonConvert.SerializeObject(jsonPatchDocument), Encoding.UTF8,
                        "application/json");

                    var response = _a.PatchResourceAsync(patchApiForCategory, stringContent).Result;


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

                }

                if (consoleKeyInfo.Key == ConsoleKey.N)
                {
                    Clear();
                    SetCursorPosition(MenuCursorPosLeft, MenuCursorPosTop);
                    WriteLine("No changes applied");
                    Thread.Sleep(1500);
                }
            }

            return edited;
        }
    }
}