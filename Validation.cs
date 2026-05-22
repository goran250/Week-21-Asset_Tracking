using System;
using System.Runtime.CompilerServices;

namespace Asset_Tracking    
{
    public static class Validation
    {

        // Gets a atring from the console and validates it so its not empty.
        public static string GetValidatedStringFromConsole(string variableName)
        {
            ColoredText.Write("\n Enter a " + variableName + ": ", ConsoleColor.Yellow);
            string result = Console.ReadLine();

            while (String.IsNullOrEmpty(result))
            {
                ColoredText.WriteLine(" " + variableName + " can't be an empty string", ConsoleColor.Red);
                ColoredText.Write(" Enter a " + variableName + ": ", ConsoleColor.Yellow);
                result = Console.ReadLine();
            }

            return result;
        }

        // Gets an integer from the console and validates it so its not empty and only contains digits, and also checks if the number is between min and max.
        public static int GetValidatedIntFromConsole(string variableName, int min, int max, bool allowNullOrEmpty)
        {
            bool isValidInteger;
            string? intValueAsString;
            int intValue;

            do
            {
                ColoredText.Write("\n Enter a " + variableName + ": ", ConsoleColor.Yellow);
                
                intValueAsString = Console.ReadLine();

                if (allowNullOrEmpty && String.IsNullOrEmpty(intValueAsString))
                {
                    return -1;
                }

                isValidInteger = int.TryParse(intValueAsString, out intValue);

                if (isValidInteger == false)
                {
                    ColoredText.WriteLine(" " + variableName + " can only contain digits and can't be empty.", ConsoleColor.Red);
                }
                else if (intValue < min || intValue > max)
                {
                    ColoredText.WriteLine(" " + variableName + " must be non-negative and higher than zero and lower or equal to " + max + ".", ConsoleColor.Red);
                    isValidInteger = false;
                }
            } while (isValidInteger == false);

            return intValue;
        }

        // Checks if a date is valid and not empty.
        // If checkNullOrEmpty is false, it allows the user to enter an empty string and returns null in that case.
        public static DateTime? GetValidatedDateFromConsole(string variableName, bool allowNullOrEmpty)
        {
            bool isDate;
            string result = "";
            DateTime date = DateTime.MinValue;

            do
            {
                ColoredText.Write("\n Enter a " + variableName + ": ", ConsoleColor.Yellow);
                result = Console.ReadLine();

                if (allowNullOrEmpty && String.IsNullOrEmpty(result))
                {
                    return null ;
                }

                if (String.IsNullOrEmpty(result))
                {
                    ColoredText.WriteLine(" You have entered an empty string for date.", ConsoleColor.Red);
                    isDate = false;
                }
                else
                {
                    isDate = DateTime.TryParse(result, out date);

                    if (isDate == false)
                    {
                        ColoredText.WriteLine(" You have not entered a valid date.", ConsoleColor.Red);
                    }
                }

            } while (isDate == false);

            return date;
        }
    }
}

