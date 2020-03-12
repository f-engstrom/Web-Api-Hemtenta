namespace API_Web_API_Kurs.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public int MenuId { get; set; }

        public string Name { get; set; }

        public string Link { get; set; }
        public int Priority { get; set; }

    }
}