using System;
using static System.Console;
using static WebAPI_Hemtenta.AuthenticationAndAuthorization;
using static WebAPI_Hemtenta.Products.ProductAdminMethods;
using static WebAPI_Hemtenta.Program;

namespace WebAPI_Hemtenta.Products
{
    class ProductMenu
    {
        public static void Menu()
        {
            bool shouldNotExit = true;

            while (shouldNotExit)
            {

                Clear();
                WriteLine("Press Esc to go back.".PadRight(Program.WindowWidth, '#'));
                WriteLine("".PadRight(Program.WindowWidth, '#'));
                SetCursorPosition(MenuCursorPosLeft, MenuCursorPosTop);
                WriteLine("1. List Products");

                SetCursorPosition(MenuCursorPosLeft, MenuCursorPosTop + 1);
                WriteLine(IsAdmin? "2. Add Product":"");

              

                ConsoleKeyInfo keyPressed = ReadKey(true);


                switch (keyPressed.Key)
                {

                    case ConsoleKey.D1:

                        Clear();

                        ListProducts();

                        break;

                    case ConsoleKey.D2:

                        if (IsAdmin)
                        {

                            Clear();

                            AddProduct(); 
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