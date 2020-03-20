using System.Collections.Generic;

namespace WebAPI_Hemtenta.Models
{
   public struct Coordinates
    {
        public int X { get; }
        public int Y { get; }

        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
            SavedCoordinates = new Dictionary<string, Coordinates>();
        }

        public Dictionary<string,Coordinates> SavedCoordinates { get; }

        public void SaveCoordinate(string name,int x,int y)
        {
            SavedCoordinates.Add(name,new Coordinates(x,y));

        }
       


    }
}
