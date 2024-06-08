using UnityEngine;

namespace BTG.Utilities
{
    public static class StringExtensions
    {
        /// <summary>
        /// Removes any suffix from the string after the specified character.
        /// </summary>
        /// <param name="source">The input string.</param>
        /// <param name="suffixIndicator">The character that indicates the start of the suffix.</param>
        /// <returns>The string without the suffix after the specified character.</returns>
        public static string RemoveSuffix(this string source, char suffixIndicator)
        {
            if (string.IsNullOrEmpty(source))
                return source;

            int index = source.IndexOf(suffixIndicator);
            if (index != -1)
            {
                return source[..index];
            }

            return source;
        }

        /// <summary>
        /// Create a method that returns a string without any white space and should have a max length of 50 characters
        /// </summary>
        /// <param name="str">the string to edit</param>
        /// <param name="length">the max length to allow</param>
        public static string RemoveWhiteSpaceAndLimitLength(this string str, int length)
        {
            return str.Replace(" ", "")[..Mathf.Min(str.Length, length)];
        }

        // Add a method that will filter the string to only supports alphanumeric values, `-`, `_` and has a maximum length of 30 characters.
        public static string FilterStringToLetterDigitDashUnderscoreMaxLength(this string str, int length)
        {
            string filteredString = "";
            foreach (char c in str)
            {
                if (char.IsLetterOrDigit(c) || c == '-' || c == '_')
                {
                    filteredString += c;
                }
            }
            return filteredString[..Mathf.Min(str.Length, length)];
        }
    }
}


