using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using WebAPI_Hemtenta.Models;
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
                    string listName = listProp.PropertyCustomName ?? listProp.PropertyName;
                    WriteLine(listName);
                    coordinates.SaveCoordinate(listName, cursorPositionLeft, cursorPositionTop);

                    IList listContents = (IList)property.GetValue(type);

                    int listOffsetLeft = listName.Length + 1;
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
                            WriteLine($"{listItemCounter}. {listItemName} :");

                            SetCursorPosition(cursorPositionLeft + 20, cursorPositionTop);
                            WriteLine($"{prop.PropertyValue}");

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
                    WriteLine($"{name} :");

                    SetCursorPosition(cursorPositionLeft + 20, cursorPositionTop);
                    WriteLine($"{prop.PropertyValue}");

                    coordinates.SaveCoordinate(prop.PropertyName, cursorPositionLeft, cursorPositionTop);

                }

                offsetTop++;
            }
        }

        public static void PrintPropertiesWithValues<T>(this T type, Coordinates coordinates, PrintSettings printSettings)
        {
            int offsetTop = 0;
            int propertyValueSpacing = printSettings.PropertiesAndValuesSpacing;
            int propertySpacing = printSettings.PropertiesSpacing;
            string chosenListProperty = printSettings.ChosenListProperty;


            foreach (var property in type.GetType().GetProperties())
            {

                if (property.PropertyType.Name == "List`1" && printSettings.PrintFullList)
                {
                    int cursorPositionLeft = coordinates.X;
                    int cursorPositionTop = coordinates.Y + offsetTop;
                    SetCursorPosition(cursorPositionLeft, cursorPositionTop);
                    Property listProp = GetPropertyNameAndValue(property, type);
                    string listName = listProp.PropertyCustomName ?? listProp.PropertyName;
                    WriteLine(listName);
                    coordinates.SaveCoordinate(listName, cursorPositionLeft, cursorPositionTop);

                    IList listContents = (IList)property.GetValue(type);

                    int listOffsetLeft = listName.Length + 1;
                    offsetTop++;
                    SetCursorPosition(coordinates.X, coordinates.Y + offsetTop);
                    WriteLine("_".PadRight(listOffsetLeft + 10, '_'));
                    offsetTop += propertySpacing;

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
                            WriteLine($"{listItemCounter}. {listItemName} :");

                            SetCursorPosition(cursorPositionLeft + propertyValueSpacing, cursorPositionTop);
                            WriteLine($"{prop.PropertyValue}");

                            coordinates.SaveCoordinate($"{listProp.PropertyName}.{listItemCounter}.{prop.PropertyName}", cursorPositionLeft, cursorPositionTop);

                            offsetTop += propertySpacing;
                        }


                        listItemCounter++;
                    }

                }
                if (property.PropertyType.Name == "List`1" && printSettings.PrintFlatList)
                {
                    int cursorPositionLeft = coordinates.X;
                    int cursorPositionTop = coordinates.Y + offsetTop;
                    SetCursorPosition(cursorPositionLeft, cursorPositionTop);
                    Property listProp = GetPropertyNameAndValue(property, type);
                    string listName = listProp.PropertyCustomName ?? listProp.PropertyName;

                    string listPropertyNameAndProperty = $"{listName} {chosenListProperty}:";
                    WriteLine(listPropertyNameAndProperty);
                    coordinates.SaveCoordinate(listName, cursorPositionLeft, cursorPositionTop);

                    IList listContents = (IList)property.GetValue(type);

                    string values = "";
                    foreach (var listObject in listContents)
                    {

                        foreach (var listObjectProperty in listObject.GetType().GetProperties())
                        {

                            if (listObjectProperty.Name.ToUpper() == chosenListProperty.ToUpper())
                            {
                                Property prop = GetPropertyNameAndValue(listObjectProperty, listObject);

                                if (listContents.IndexOf(listObject) == 0)
                                {
                                    values += $"{prop.PropertyValue}";

                                }
                                else
                                {
                                    values += $", {prop.PropertyValue}";

                                }


                            }

                        }


                    }

                    SetCursorPosition(cursorPositionLeft + listPropertyNameAndProperty.Length + 1, cursorPositionTop);
                    WriteLine(values);

                }

                else
                {
                    int cursorPositionLeft = coordinates.X;
                    int cursorPositionTop = coordinates.Y + offsetTop;
                    Property prop = GetPropertyNameAndValue(property, type);

                    string name = prop.PropertyCustomName ?? prop.PropertyName;

                    SetCursorPosition(cursorPositionLeft, cursorPositionTop);
                    WriteLine($"{name} :");

                    SetCursorPosition(cursorPositionLeft + propertyValueSpacing, cursorPositionTop);
                    WriteLine($"{prop.PropertyValue}");

                    coordinates.SaveCoordinate(prop.PropertyName, cursorPositionLeft, cursorPositionTop);

                }

                offsetTop += propertySpacing;
            }
        }


        public static void PrintProperties<T>(this T type, Coordinates coordinates, PrintSettings printSettings)
        {
            int offsetTop = 0;
            int propertyValueSpacing = printSettings.PropertiesAndValuesSpacing;
            int propertySpacing = printSettings.PropertiesSpacing;
            string chosenListProperty = printSettings.ChosenListProperty;


            foreach (var property in type.GetType().GetProperties())
            {

                if (property.PropertyType.Name == "List`1" && printSettings.PrintFullList)
                {
                    int cursorPositionLeft = coordinates.X;
                    int cursorPositionTop = coordinates.Y + offsetTop;
                    SetCursorPosition(cursorPositionLeft, cursorPositionTop);
                    Property listProp = GetPropertyNameAndValue(property, type);
                    string listName = listProp.PropertyCustomName ?? listProp.PropertyName;
                    WriteLine(listName);
                    coordinates.SaveCoordinate(listName, cursorPositionLeft, cursorPositionTop);

                }
                else
                {
                    int cursorPositionLeft = coordinates.X;
                    int cursorPositionTop = coordinates.Y + offsetTop;
                    Property prop = GetPropertyNameAndValue(property, type);

                    string name = prop.PropertyCustomName ?? prop.PropertyName;

                    SetCursorPosition(cursorPositionLeft, cursorPositionTop);
                    WriteLine($"{name} :");

                   

                    coordinates.SaveCoordinate(prop.PropertyName, cursorPositionLeft, cursorPositionTop);

                }

                offsetTop += propertySpacing;
            }
        }



        public static void PrintProperties<T>(this T type, Coordinates coordinates)
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
                    string listName = listProp.PropertyCustomName ?? listProp.PropertyName;
                    WriteLine(listName);
                    coordinates.SaveCoordinate(listName, cursorPositionLeft, cursorPositionTop);
                }
                else
                {
                    int cursorPositionLeft = coordinates.X;
                    int cursorPositionTop = coordinates.Y + offsetTop;
                    Property prop = GetPropertyNameAndValue(property, type);

                    string name = prop.PropertyCustomName ?? prop.PropertyName;

                    SetCursorPosition(cursorPositionLeft, cursorPositionTop);
                    WriteLine($"{name} :");


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

                    if (description.Description != null)
                    {
                        propertyCustomName = description.Description;

                    }

                }
            }

            return new Tuple<string, string>(propertyName, propertyCustomName);
        }


    }


    public class PrintSettings
    {

        public int PropertiesSpacing { get; set; }
        public int PropertiesAndValuesSpacing { get; set; }
        public string ChosenListProperty { get; set; }
        public bool PrintFullList { get; set; }
        public bool PrintFlatList { get; set; }


    }
}