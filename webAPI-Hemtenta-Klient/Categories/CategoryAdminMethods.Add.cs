using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using WebAPI_Hemtenta.Models;
using static System.Console;
using static WebAPI_Hemtenta.Program;
using static WebAPI_Hemtenta.Products.HelperMethods;



namespace WebAPI_Hemtenta.Categories
{
    partial class CategoryAdminMethods
    {
        static Api _a = new Api();


        internal static void AddCategory()
        {


            Clear();


            CategoryDto category = CreateCategoryFromInput();

            if (category != null)
            {


                HttpResponseMessage response = _a.PostResourceAsync(Api.CategoryApi, category).Result;


                if (response.IsSuccessStatusCode)
                {
                    Clear();
                    SetCursorPosition(MenuCursorPosLeft, MenuCursorPosTop);
                    WriteLine($"Category added sucessfully.");
                    Thread.Sleep(2000);

                }

                else
                {

                    Clear();
                    SetCursorPosition(MenuCursorPosLeft, MenuCursorPosTop);
                    WriteLine($"Something went wrong with adding the category.");
                    Thread.Sleep(2000);
                }




            }



        }





        private static CategoryDto CreateCategoryFromInput()
        {
            CategoryDto category = new CategoryDto();

            Clear();
            WriteLine("Press Esc to when done editing.".PadRight(Program.WindowWidth, '#'));
            WriteLine("".PadRight(Program.WindowWidth, '#'));
            OptionsPrinter("    ");


            Coordinates categoryPropertiesCoordinates = new Coordinates(ContentCursorPosLeft, ContentCursorPosTop);

            category.PrintProperties(categoryPropertiesCoordinates);



            void EraseOldText(int length)
            {
                int cursorLeft = CursorLeft;
                int cursorTop = CursorTop;
                Write(" ".PadRight(length));
                SetCursorPosition(cursorLeft, cursorTop);
            }


            List<string> propertyNames = categoryPropertiesCoordinates.SavedCoordinates.Keys.ToList();



            int moveCursor = 0;

            bool shouldNotExit = true;

            int propertysValuesSpacing = 10;

            ConsoleKeyInfo consoleKeyInfo;


            bool correktKey;

            while (shouldNotExit)
            {





                SetCursorPosition(categoryPropertiesCoordinates.SavedCoordinates[propertyNames[moveCursor]].X + propertysValuesSpacing, categoryPropertiesCoordinates.SavedCoordinates[propertyNames[moveCursor]].Y);

                do
                {
                    consoleKeyInfo = ReadKey(true);

                    correktKey = !(consoleKeyInfo.Key == ConsoleKey.UpArrow || consoleKeyInfo.Key == ConsoleKey.DownArrow || consoleKeyInfo.Key == ConsoleKey.Escape || consoleKeyInfo.Key == ConsoleKey.Enter);

                } while (correktKey);

                switch (consoleKeyInfo.Key)
                {

                    case ConsoleKey.UpArrow:

                        if (moveCursor > 0)
                        {
                            moveCursor--;
                        }

                        break;

                    case ConsoleKey.DownArrow:

                        if (moveCursor < propertyNames.Count - 1)
                        {
                            moveCursor++;
                        }

                        break;

                    case ConsoleKey.Enter:

                        string currentProperty = propertyNames[moveCursor];


                        if (!currentProperty.ToUpper().Contains("ID"))
                        {

                            EraseOldText(100);

                            string input = ReadLine();


                            foreach (var property in category.GetType().GetProperties())
                            {
                                if (property.Name == currentProperty)
                                {
                                    property.SetValue(category, input);
                                }
                            }


                        }


                        break;

                    case ConsoleKey.Escape:
                        shouldNotExit = false;

                        break;

                }


                SetCursorPosition(categoryPropertiesCoordinates.SavedCoordinates[propertyNames[moveCursor]].X + propertysValuesSpacing, categoryPropertiesCoordinates.SavedCoordinates[propertyNames[moveCursor]].Y);





            }


            if (category.ImageUrl == null && category.Name == null) return null;

            bool b;

            OptionsPrinter("Save category? (Y)es (N)o");
            do
            {
                consoleKeyInfo = ReadKey(true);

                b = !(consoleKeyInfo.Key == ConsoleKey.Y || consoleKeyInfo.Key == ConsoleKey.N);

            } while (b);

            switch (consoleKeyInfo.Key)
            {
                case ConsoleKey.Y:
                    Clear();


                    return category;
                case ConsoleKey.N:
                    Clear();
                    SetCursorPosition(MenuCursorPosLeft, MenuCursorPosTop);

                    WriteLine("No changes applied");
                    Thread.Sleep(1500);

                    return null;
                default:
                    return null;
            }
        }
    }
}