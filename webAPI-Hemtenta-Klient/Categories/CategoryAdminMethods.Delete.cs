using System;
using System.Threading;
using WebAPI_Hemtenta.Models;
using WebAPI_Hemtenta.Products;
using static System.Console;
using static WebAPI_Hemtenta.Program;

namespace WebAPI_Hemtenta.Categories
{
    partial class CategoryAdminMethods
    {
        internal static void DeleteCategory(Category category)
        {
            Clear();
            bool b;

            Coordinates coordinates = new Coordinates(ContentCursorPosLeft, ContentCursorPosTop);
            category.PrintPropertiesWithValues(coordinates);
            int cursorTop = CursorTop;


            HelperMethods.OptionsPrinter("Are you sure you want to delete this category? (Y)es or (N)o");

            ConsoleKeyInfo consoleKeyInfo;

            do
            {
                consoleKeyInfo = ReadKey(true);

                b = !(consoleKeyInfo.Key == ConsoleKey.Y || consoleKeyInfo.Key == ConsoleKey.N);
            } while (b);

            if (consoleKeyInfo.Key == ConsoleKey.Y)
            {
                var response = _a.DeleteResourceAsync(Api.CategoryApi, category.Id).Result;

                if (response.IsSuccessStatusCode)
                {
                    Clear();
                    SetCursorPosition(MenuCursorPosLeft, MenuCursorPosTop);
                    WriteLine($"Category sucessfully deleted.");
                    Thread.Sleep(2000);
                }
                else
                {
                    Clear();
                    SetCursorPosition(MenuCursorPosLeft, MenuCursorPosTop);
                    WriteLine($"Something went wrong with deleting the category.");
                    Thread.Sleep(2000);
                }
            }
        }
    }
}