using System;

namespace WebAPI_Hemtenta.Products
{
    static class HelperMethods
    {
        public static int? GetIdOrNull()
        {
            string idString;
            int id;
            OptionsPrinter("ID: ");
            idString = Console.ReadLine();
            bool pass = Int32.TryParse(idString, out id);
            if (pass)
            {
                return id;
            }
            else
            {
                return null;
            }
        }


        public static void EraseOldText(int length)
        {
            int cursorLeft = Console.CursorLeft;
            int cursorTop = Console.CursorTop;
            Console.Write(" ".PadRight(length));
            Console.SetCursorPosition(cursorLeft, cursorTop);
        }


        public static void OptionsPrinter(string options)
        {
            string message = $"{options}";

            Console.SetCursorPosition(Program.OptionsCursorPosLeft, Program.OptionsCursorPosTop - 3);
            Console.Write("".PadRight(Program.WindowWidth, '#'));
            Console.SetCursorPosition(Program.OptionsCursorPosLeft, Program.OptionsCursorPosTop - 2);
            Console.Write("".PadRight(Program.WindowWidth, '#'));
            Console.SetCursorPosition(Program.OptionsCursorPosLeft, Program.OptionsCursorPosTop - 1);
            Console.Write("".PadRight(Program.WindowWidth, '#'));
            Console.SetCursorPosition(Program.OptionsCursorPosLeft, Program.OptionsCursorPosTop);
            Console.Write("".PadRight(Program.WindowWidth, ' '));
            Console.SetCursorPosition(Program.OptionsCursorPosLeft, Program.OptionsCursorPosTop);
            Console.WriteLine(message);
            Console.Write("".PadRight(Program.WindowWidth, '#'));
            Console.SetCursorPosition(Program.OptionsCursorPosLeft + message.Length + 1, Program.OptionsCursorPosTop);

        }
    }
}