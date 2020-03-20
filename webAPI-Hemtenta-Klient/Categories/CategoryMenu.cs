using System;
using static System.Console;
using static WebAPI_Hemtenta.AuthenticationAndAuthorization;
using static WebAPI_Hemtenta.Program;


namespace WebAPI_Hemtenta.Categories
{
    class CategoryMenu
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
                WriteLine("1. List Categories");
                SetCursorPosition(MenuCursorPosLeft, MenuCursorPosTop + 1);

                WriteLine(IsAdmin ? "2. Add Category":"");
             

                ConsoleKeyInfo keyPressed = ReadKey(true);


                switch (keyPressed.Key)
                {

                    case ConsoleKey.D1:

                        Clear();

                        CategoryAdminMethods.ListCategories();

                        break;

                    case ConsoleKey.D2:

                        if (IsAdmin)
                        {
                            Clear();


                            CategoryAdminMethods.AddCategory();

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