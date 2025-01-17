﻿using System;
using WebAPI_Hemtenta.Categories;
using WebAPI_Hemtenta.Products;
using static System.Console;
using static WebAPI_Hemtenta.Program;

namespace WebAPI_Hemtenta
{
    class MainMenu
    {

        public static void Menu()
        {
            bool shouldNotExit = true;

            while (shouldNotExit)
            {

                Clear();

                SetCursorPosition(MenuCursorPosLeft, MenuCursorPosTop);

                WriteLine("1. Products");
                SetCursorPosition(MenuCursorPosLeft, MenuCursorPosTop + 1);

                WriteLine("2. Categories");
                SetCursorPosition(MenuCursorPosLeft, MenuCursorPosTop + 2);

                WriteLine("3. Exit");



                ConsoleKeyInfo keyPressed = ReadKey(true);


                switch (keyPressed.Key)
                {

                    case ConsoleKey.D1:

                        Clear();

                        ProductMenu.Menu();

                        break;

                    case ConsoleKey.D2:

                        Clear();
                        CategoryMenu.Menu();

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