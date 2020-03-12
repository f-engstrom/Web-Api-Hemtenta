using System.Collections.Generic;

namespace API_Web_API_Kurs.Models
{
    public class Menu
    {

        public int Id { get; set; }
        public string Name { get; set; }

        public List<MenuItem> MenuItems { get; set; }
    }
}
