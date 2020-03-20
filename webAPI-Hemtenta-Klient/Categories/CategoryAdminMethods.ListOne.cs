using System;
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
        private static void ListCategory(Category category)
        {
            bool shouldNotExit = true;
            bool correktKey;
            ConsoleKeyInfo keyPressed; 
            string uriString = $"{Api.CategoryApi}/{category.Id}";
            Uri ApiForCategory = new Uri(uriString);


            while (shouldNotExit)
            {
                Clear();
                WriteLine("Press Esc to go back.".PadRight(Program.WindowWidth, '#'));
                WriteLine("".PadRight(Program.WindowWidth, '#'));
                Coordinates coordinates = new Coordinates(ContentCursorPosTop, ContentCursorPosTop);
                category.PrintPropertiesWithValues(coordinates);
                int cursorTop = CursorTop;


                if (IsAdmin)
                {
                    OptionsPrinter("(D)elete (E)dit");

                    do
                    {
                        keyPressed = ReadKey(true);

                        correktKey = !(keyPressed.Key == ConsoleKey.D || keyPressed.Key == ConsoleKey.Escape ||
                                       keyPressed.Key == ConsoleKey.E);
                    } while (correktKey);
                }
                else
                {
                    do
                    {
                        keyPressed = ReadKey(true);

                        correktKey = keyPressed.Key != ConsoleKey.Escape;
                    } while (correktKey);
                }

                SetCursorPosition(coordinates.X, cursorTop + 1);


                switch (keyPressed.Key)
                {
                    case ConsoleKey.D:

                        if (IsAdmin)
                        {
                            Clear();

                            DeleteCategory(category);
                            shouldNotExit = false;

                        }

                        break;


                    case ConsoleKey.E:

                        if (IsAdmin)
                        {
                            Clear();

                            if (EditCategory(category))
                                category = _a.GetResourceAsync<Category>(ApiForCategory).Result;

                        }

                        break;

                    case ConsoleKey.Escape:

                        Clear();

                        shouldNotExit = false;

                        break;
                }
            }
        }
    }
}