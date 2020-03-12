using System;
using static System.Console;

namespace WebAPI_Hemtenta
{
    class CategoryMenu
    {
        public static void Menu()
        {
            bool shouldNotExit = true;

            while (shouldNotExit)
            {

                Clear();

                SetCursorPosition(Program.MenuCursorPosLeft, Program.MenuCursorPosTop);
                WriteLine("1. List Categories");
                SetCursorPosition(Program.MenuCursorPosLeft, Program.MenuCursorPosTop+1);

                WriteLine("2. Add Category");
                SetCursorPosition(Program.MenuCursorPosLeft, Program.MenuCursorPosTop+2);

                WriteLine("3. Delete Category");
                SetCursorPosition(Program.MenuCursorPosLeft, Program.MenuCursorPosTop+3);

                WriteLine("4. Exit");

                ConsoleKeyInfo keyPressed = ReadKey(true);


                switch (keyPressed.Key)
                {

                    case ConsoleKey.D1:

                        Clear();

                        CategoryAdminMethods.ListCategories();

                        break;

                    case ConsoleKey.D2:

                        Clear();


                        CategoryAdminMethods.AddCategory();
                        break;

                    case ConsoleKey.D3:

                        Clear();


                        CategoryAdminMethods.DeleteCategory();
                        break;

                    case ConsoleKey.D4:

                        Clear();


                        shouldNotExit = false;
                        break;




                }



            }




        }
    }
}