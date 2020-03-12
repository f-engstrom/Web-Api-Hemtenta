using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using static System.Console;

namespace WebAPI_Hemtenta
{
    static class MyExtensions
    {
        public static void PrintPropertiesWithValues<T>(this T type, Coordinates coordinates)
        {
            int offsetTop = 0;

          

            foreach (var property in type.GetType().GetProperties())
            {

                if (property.PropertyType.Name == "List`1")
                {
                    int cursorPositionLeft = coordinates.X;
                    int cursorPositionTop = coordinates.Y + offsetTop;
                    SetCursorPosition(cursorPositionLeft, cursorPositionTop);
                    Property listProp = GetPropertyNameAndValue(property, type);
                    string ListName = listProp.PropertyCustomName ?? listProp.PropertyName;
                    Console.WriteLine(ListName);
                    coordinates.SaveCoordinate(ListName, cursorPositionLeft, cursorPositionTop);

                    IList listContents = (IList)property.GetValue(type);

                    int listOffsetLeft = ListName.Length + 1;
                    offsetTop++;
                    SetCursorPosition(coordinates.X, coordinates.Y + offsetTop);
                    WriteLine("_".PadRight(listOffsetLeft + 10, '_'));
                    offsetTop++;

                    int listItemCounter = 1;
                    foreach (var listObject in listContents)
                    {

                        foreach (var listObjectProperty in listObject.GetType().GetProperties())
                        {
                             cursorPositionLeft = coordinates.X + listOffsetLeft;
                             cursorPositionTop = coordinates.Y + offsetTop;

                            Property prop = GetPropertyNameAndValue(listObjectProperty, listObject);
                            string listItemName = prop.PropertyCustomName ?? prop.PropertyName;

                            SetCursorPosition(cursorPositionLeft, cursorPositionTop);
                            Console.WriteLine($"{listItemCounter}. {listItemName} :");

                            SetCursorPosition(cursorPositionLeft+20, cursorPositionTop);
                            Console.WriteLine($"{prop.PropertyValue}");

                            coordinates.SaveCoordinate($"{listProp.PropertyName}.{listItemCounter}.{prop.PropertyName}", cursorPositionLeft, cursorPositionTop);

                            offsetTop++;
                        }


                        listItemCounter++;
                    }

                }
                else
                {
                    int cursorPositionLeft = coordinates.X;
                    int cursorPositionTop = coordinates.Y + offsetTop;
                    Property prop = GetPropertyNameAndValue(property, type);

                    string name = prop.PropertyCustomName ?? prop.PropertyName;

                    SetCursorPosition(cursorPositionLeft, cursorPositionTop);
                    Console.WriteLine($"{name} :");

                    SetCursorPosition(cursorPositionLeft+20, cursorPositionTop);
                    Console.WriteLine($"{prop.PropertyValue}");

                    coordinates.SaveCoordinate(prop.PropertyName, cursorPositionLeft, cursorPositionTop);

                }

                offsetTop++;
            }
        }


        private class Property
        {
            public string PropertyName { get; set; }
            public string PropertyCustomName { get; set; }
            public dynamic PropertyValue { get; set; }

            public Property(string propertyName, string propertyCustomName, dynamic propertyValue)
            {
                PropertyName = propertyName;
                PropertyCustomName = propertyCustomName;
                PropertyValue = propertyValue;
            }

        }



        private static Property GetPropertyNameAndValue(PropertyInfo property, Object type)
        {
            var propertyNames = GetPropertyNameAndCustomName(property);

            Property prop = new Property(propertyNames.Item1, propertyNames.Item2, property.GetValue(type));

            return prop;

        }



        static Tuple<string, string> GetPropertyNameAndCustomName(PropertyInfo property)
        {
            string propertyName = property.Name;
            string propertyCustomName = null;

            if (property.GetCustomAttributes().Any())
            {
                foreach (var attribute in property.GetCustomAttributes())
                {
                    DescriptionAttribute description = attribute as DescriptionAttribute;


                    propertyCustomName = description.Description;
                }
            }

            return new Tuple<string, string>(propertyName, propertyCustomName);
        }


    }
}