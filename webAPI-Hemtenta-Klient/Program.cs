using System.Threading;
using static System.Console;
using static WebAPI_Hemtenta.AuthenticationAndAuthorization;

namespace WebAPI_Hemtenta
{
    class Program
    {
        public const int MenuCursorPosLeft = 35;
        public const int MenuCursorPosTop = 17;
        public const int ContentCursorPosLeft = 5;
        public const int ContentCursorPosTop = 10;
        public const int OptionsCursorPosLeft = 0;
        public const int OptionsCursorPosTop = 39;
        public const int WindowWidth = 130;
        public const int WindowHeight = 41;
        static void Main(string[] args)
        {

            bool authenticated = false;
            SetWindowSize(WindowWidth, WindowHeight);
            

            do
            {
                Clear();

                if (UserManager())
                {

                    authenticated = true;

                }
                else
                {
                    Clear();
                    SetCursorPosition(MenuCursorPosLeft,MenuCursorPosTop);
                    WriteLine("Authentication was unsucessfull.");
                    Thread.Sleep(1500);


                }




            } while (!authenticated);

            Clear();

            MainMenu.Menu();

        }


    }
}
