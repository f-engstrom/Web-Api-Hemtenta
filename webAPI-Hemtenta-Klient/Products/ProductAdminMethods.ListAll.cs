using System;
using System.Collections.Generic;
using System.Linq;
using WebAPI_Hemtenta.Models;
using static WebAPI_Hemtenta.Program;

namespace WebAPI_Hemtenta.Products
{
    static partial class ProductAdminMethods
    {
        public static Api _a = new Api();

        public static void ListProducts()
        {
            bool shouldNotExit = true;
            bool shouldPrint = true;

            Console.Clear();
            do
            {
                List<Product> products = _a.GetResourceAsync<List<Product>>(Api.ProductApi).Result;

                if (shouldPrint)
                {

                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine("Press Esc to go back.".PadRight(WindowWidth, '#'));
                    Console.WriteLine("".PadRight(WindowWidth, '#'));

                    PrintProducts(products);
                }

                Product chosenProduct;
                int? id;

                ConsoleKeyInfo keyPressed; 
                bool correctKey;

                if (AuthenticationAndAuthorization.IsAdmin)
                {
                    HelperMethods.OptionsPrinter("(V)iew (D)elete (E)dit");

                    do
                    {
                        keyPressed = Console.ReadKey(true);

                        correctKey = !(keyPressed.Key == ConsoleKey.V || keyPressed.Key == ConsoleKey.D ||
                                       keyPressed.Key == ConsoleKey.Escape || keyPressed.Key == ConsoleKey.E);

                    } while (correctKey);

                }
                else
                {
                    HelperMethods.OptionsPrinter("(V)iew");
                    do
                    {
                        keyPressed = Console.ReadKey(true);

                        correctKey = !(keyPressed.Key == ConsoleKey.V || keyPressed.Key == ConsoleKey.Escape);

                    } while (correctKey);
                }





                switch (keyPressed.Key)
                {

                    case ConsoleKey.V:

                        shouldPrint = false;

                        id = HelperMethods.GetIdOrNull();


                        if (id != null)
                        {
                            chosenProduct = products.FirstOrDefault(x => x.Id == id);

                            Console.Clear();

                            ListProduct(chosenProduct);

                            Console.Clear();
                            shouldPrint = true;


                        }


                        break;

                    case ConsoleKey.D:

                        if (AuthenticationAndAuthorization.IsAdmin)
                        {
                            shouldPrint = false;

                            id = HelperMethods.GetIdOrNull();

                            if (id != null)
                            {
                                chosenProduct = products.FirstOrDefault(x => x.Id == id);

                                Console.Clear();

                                DeleteProduct(chosenProduct);
                                Console.Clear();
                                shouldPrint = true;

                            }

                        }

                        break;


                    case ConsoleKey.E:

                        if (AuthenticationAndAuthorization.IsAdmin)
                        {
                            shouldPrint = false;

                            id = HelperMethods.GetIdOrNull();

                            if (id != null)
                            {
                                chosenProduct = products.FirstOrDefault(x => x.Id == id);


                                Console.Clear();

                                EditProduct(chosenProduct);
                                Console.Clear();
                                shouldPrint = true;


                            }
                        }

                        break;

                    case ConsoleKey.Escape:

                        Console.Clear();

                        shouldNotExit = false;

                        break;



                }

            } while (shouldNotExit);






        }

        private static void PrintProducts(List<Product> products)
        {
            Console.SetCursorPosition(ContentCursorPosLeft, ContentCursorPosTop);
            int nextLine = 0;
            int leftAdjustment = ContentCursorPosLeft;
            int longestStringLength = 0;

            var longestName = products.Select(n => n.Name).Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur).Length;
            var longestId = products.Select(n => n.Id.ToString()).Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur).Length;


            foreach (var product in products)
            {
                string id = "|Id: " + $"{product.Id}".PadRight(longestId) + "|";
                string name = "Name: " + $"{product.Name}".PadRight(longestName) + "|";

                string idAndName = id + name;

                Console.WriteLine(idAndName);

                nextLine++;

                if (idAndName.Length > longestStringLength)
                {
                    longestStringLength = idAndName.Length;
                }

                if (nextLine > 15)
                {
                    leftAdjustment += longestStringLength + 2;
                    nextLine = 0;
                    longestStringLength = 0;
                }

                Console.SetCursorPosition(leftAdjustment, ContentCursorPosTop + nextLine);
            }
        }
    }
}