using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Uml2Node.Generator.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Changes the case of the string to Startcase.
        /// </summary>
        public static string ToStartCase(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }

        /// <summary>
        /// Changes the case of the string to PascalCase.
        /// </summary>
        public static string ToPascalCase(this string input)
        {
            string[] words = Regex.Split(input, @"(?<!^)(?=[A-Z])");

            return String.Join("", words.Select(e => e.ToStartCase()));
        }

        /// <summary>
        /// Changes the case of the string to PascalCase.
        /// </summary>
        public static string ToCamelCase(this string input)
        {
            string[] words = input.ToLower().Split(' ');

            List<string> camelWords = new List<string>() { words[0].ToLower() };
            camelWords.AddRange(words.Skip(1).Select(e => e.ToStartCase()));

            return String.Join("", camelWords);
        }

        /// <summary>
        /// Changes the case of the string to dash-case.
        /// </summary>
        public static string ToDashCase(this string input)
        {
            string[] words = Regex.Split(input, @"(?<!^)(?=[A-Z])");

            return String.Join("-", words.Select(e => e.ToLower()));
        }
    }
}
