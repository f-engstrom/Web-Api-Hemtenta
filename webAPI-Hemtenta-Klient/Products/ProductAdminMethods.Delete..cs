using System;
using System.Threading;
using WebAPI_Hemtenta.Models;
using static System.Console;
using static WebAPI_Hemtenta.Products.HelperMethods;
using static WebAPI_Hemtenta.Program;

namespace WebAPI_Hemtenta.Products
{
    static partial class ProductAdminMethods
    {
        public static void DeleteProduct(Product product)
        {
            Clear();
            bool b;

            Coordinates coordinates = new Coordinates(ContentCursorPosLeft, ContentCursorPosTop);
            product.PrintPropertiesWithValues(coordinates);

            int cursorTop = CursorTop;


            OptionsPrinter("Are you sure you want to delete this product? (Y)es or (N)o");

            ConsoleKeyInfo consoleKeyInfo;

            do
            {
                consoleKeyInfo = ReadKey(true);

                b = !(consoleKeyInfo.Key == ConsoleKey.Y || consoleKeyInfo.Key == ConsoleKey.N);
            } while (b);

            if (consoleKeyInfo.Key == ConsoleKey.Y)
            {
                var response = _a.DeleteResourceAsync(Api.ProductApi, product.Id).Result;
                if (response.IsSuccessStatusCode)
                {
                    Clear();
                    SetCursorPosition(MenuCursorPosLeft, MenuCursorPosTop);
                    WriteLine($"Poduct sucessfully deleted.");
                    Thread.Sleep(2000);
                }
                else
                {
                    Clear();
                    SetCursorPosition(MenuCursorPosLeft, MenuCursorPosTop);
                    WriteLine($"Something went wrong with deleting the product.");
                    Thread.Sleep(2000);
                }
            }
        }
    }
}