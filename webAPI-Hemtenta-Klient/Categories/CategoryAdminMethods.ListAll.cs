using System;
using System.Collections.Generic;
using System.Linq;
using WebAPI_Hemtenta.Models;
using static System.Console;
using static WebAPI_Hemtenta.AuthenticationAndAuthorization;
using static WebAPI_Hemtenta.Products.HelperMethods;
using static WebAPI_Hemtenta.Program;

namespace WebAPI_Hemtenta.Categories
{
    partial class CategoryAdminMethods
    {
        public static void ListCategories()
        {
            bool shouldNotExit = true;
            bool shouldPrint = true;
            List<Category> categories;


            Clear();
            do
            {
                categories = _a.GetResourceAsync<List<Category>>(Api.CategoryApi).Result;

                if (shouldPrint)
                {
                    SetCursorPosition(0, 0);

                    WriteLine("Press Esc to go back.".PadRight(Program.WindowWidth, '#'));
                    WriteLine("".PadRight(Program.WindowWidth, '#'));

                    PrintCategories(categories);
                    shouldPrint = false;

                }


                Category chosenCategory;
                int? id;

                ConsoleKeyInfo keyPressed;
                bool correctKey;

                if (IsAdmin)
                {
                    OptionsPrinter("(V)iew (D)elete (E)dit");
                    do
                    {
                        keyPressed = ReadKey(true);

                        correctKey = !(keyPressed.Key == ConsoleKey.V || keyPressed.Key == ConsoleKey.D ||
                                       keyPressed.Key == ConsoleKey.Escape || keyPressed.Key == ConsoleKey.E);
                    } while (correctKey);
                }
                else
                {
                    OptionsPrinter("(V)iew");
                    do
                    {
                        keyPressed = ReadKey(true);

                        correctKey = !(keyPressed.Key == ConsoleKey.V || keyPressed.Key == ConsoleKey.Escape);
                    } while (correctKey);
                }


                switch (keyPressed.Key)
                {
                    case ConsoleKey.V:

                        id = GetIdOrNull();


                        if (null != id)
                        {
                            chosenCategory = categories.FirstOrDefault(x => x.Id == id);

                            Clear();

                            ListCategory(chosenCategory);
                            Clear();
                            shouldPrint = true;
                        }


                        break;

                    case ConsoleKey.D:

                        if (IsAdmin)
                        {
                            id = GetIdOrNull();


                            if (id != null)
                            {
                                chosenCategory = categories.FirstOrDefault(x => x.Id == id);

                                Clear();

                                DeleteCategory(chosenCategory);
                                Clear();
                                shouldPrint = true;
                            }
                        }

                        break;


                    case ConsoleKey.E:

                        if (IsAdmin)
                        {
                            id = GetIdOrNull();

                            if (id != null)
                            {
                                chosenCategory = categories.FirstOrDefault(x => x.Id == id);

                                Clear();

                                EditCategory(chosenCategory);
                                Clear();
                                shouldPrint = true;
                            }
                        }

                        break;

                    case ConsoleKey.Escape:

                        Clear();

                        shouldNotExit = false;

                        break;
                }
            } while (shouldNotExit);
        }

        private static void PrintCategories(List<Category> categories)
        {
            SetCursorPosition(ContentCursorPosLeft, ContentCursorPosTop);
            int nextLine = 0;
            int leftAdjustment = ContentCursorPosLeft;
            int longestStringLength = 0;

            var longestName = categories.Select(n => n.Name).Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur).Length;
            var longestId = categories.Select(n => n.Id.ToString()).Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur).Length;

            foreach (var category in categories)
            {
                string id = "|Id: " + $"{category.Id}".PadRight(longestId) + "|";
                string name = "Name: " + $"{category.Name}".PadRight(longestName) + "|";

                string idAndName = id + name;

                WriteLine(idAndName);

                nextLine++;

                if (idAndName.Length > longestStringLength)
                {
                    longestStringLength = idAndName.Length;
                }

                if (nextLine > 10)
                {
                    leftAdjustment += longestStringLength + 2;
                    nextLine = 0;
                    longestStringLength = 0;
                }

                SetCursorPosition(leftAdjustment, ContentCursorPosTop + nextLine);
            }
        }
    }
}