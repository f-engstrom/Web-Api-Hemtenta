using System;
using static System.Console;

namespace WebAPI_Hemtenta
{
    class ProductMenu
    {
        public static void Menu()
        {
            bool shouldNotExit = true;

            while (shouldNotExit)
            {

                Clear();
                SetCursorPosition(Program.MenuCursorPosLeft, Program.MenuCursorPosTop);
                WriteLine("1. List Products");

                SetCursorPosition(Program.MenuCursorPosLeft, Program.MenuCursorPosTop + 1);
                WriteLine("2. Add Product");

                SetCursorPosition(Program.MenuCursorPosLeft, Program.MenuCursorPosTop + 3);
                WriteLine("3. Exit");

                ConsoleKeyInfo keyPressed = ReadKey(true);


                switch (keyPressed.Key)
                {

                    case ConsoleKey.D1:

                        Clear();

                        ProductAdminMethods.ListProducts();

                        break;

                    case ConsoleKey.D2:

                        Clear();

                        ProductAdminMethods.AddProduct();

                        break;


                    case ConsoleKey.D3:

                        Clear();

                        shouldNotExit = false;

                        break;



                }

            }
        }
    }
}