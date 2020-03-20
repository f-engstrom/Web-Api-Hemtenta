using System;
using WebAPI_Hemtenta.Models;
using static System.Console;
using static WebAPI_Hemtenta.AuthenticationAndAuthorization;
using static WebAPI_Hemtenta.Products.HelperMethods;
using static WebAPI_Hemtenta.Program;

namespace WebAPI_Hemtenta.Products
{
    static partial class ProductAdminMethods
    {
        public static void ListProduct(Product product)
        {
            bool shouldNotExit = true;
            ConsoleKeyInfo keyPressed;
            bool correctKey;
            string uriString = $"{Api.CategoryApi}/{product.Id}";
            Uri ApiForProduct = new Uri(uriString);



            while (shouldNotExit)
            {
                Clear();
                WriteLine("Press Esc to go back.".PadRight(Program.WindowWidth, '#'));
                WriteLine("".PadRight(Program.WindowWidth, '#'));
                Coordinates coordinates = new Coordinates(ContentCursorPosLeft, ContentCursorPosTop);
                product.PrintPropertiesWithValues(coordinates);
                int cursorTop = CursorTop;


                if (IsAdmin)
                {
                    OptionsPrinter("(D)elete (E)dit");

                    do
                    {
                        keyPressed = ReadKey(true);

                        correctKey = !(keyPressed.Key == ConsoleKey.D || keyPressed.Key == ConsoleKey.Escape ||
                                       keyPressed.Key == ConsoleKey.E);
                    } while (correctKey);
                }
                else
                {
                    do
                    {
                        keyPressed = ReadKey(true);

                        correctKey = !(keyPressed.Key == ConsoleKey.Escape);
                    } while (correctKey);
                }


                switch (keyPressed.Key)
                {
                    case ConsoleKey.D:

                        if (IsAdmin)
                        {
                            Clear();

                            DeleteProduct(product);
                            shouldNotExit = false;

                        }

                        break;


                    case ConsoleKey.E:

                        if (IsAdmin)
                        {
                            Clear();

                            if (EditProduct(product))
                                product = _a.GetResourceAsync<Product>(ApiForProduct).Result;

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