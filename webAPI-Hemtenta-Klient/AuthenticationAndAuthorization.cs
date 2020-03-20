using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Newtonsoft.Json;
using static System.Console;
using static WebAPI_Hemtenta.Program;

namespace WebAPI_Hemtenta
{
    class AuthenticationAndAuthorization
    {
        public static bool IsAdmin { get; private set; }
        public static JwtSecurityToken Token { get; private set; } =new JwtSecurityToken();

        public static bool UserManager()
        {
            Credentials credentials = new Credentials();
            Api a = new Api();
           

                SetCursorPosition(MenuCursorPosLeft, MenuCursorPosTop);
                WriteLine("Username: ");
                SetCursorPosition(MenuCursorPosLeft, MenuCursorPosTop + 1);
                WriteLine("Password: ");
                SetCursorPosition(MenuCursorPosLeft + "Username: ".Length, MenuCursorPosTop);
                credentials.UserName = ReadLine();
                SetCursorPosition(MenuCursorPosLeft + "Password: ".Length, MenuCursorPosTop + 1);
                credentials.Password = ReadLine();

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                var response = a.PostResourceAsync(Api.TokenApi, credentials).Result;
            if (response.IsSuccessStatusCode)
            {

                var tokenString = response.Content.ReadAsStringAsync().Result;
                var definition = new { token = "" };
                var token = JsonConvert.DeserializeAnonymousType(tokenString, definition);
                Token = tokenHandler.ReadToken(token.token) as JwtSecurityToken;

                var isAdmin = Token.Claims.FirstOrDefault(claim => claim.Type == "hasRole").Value;

                IsAdmin = isAdmin == "admin";

                return true;
            }

            return false;


        }


        private class Credentials
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }


      

    }
}
