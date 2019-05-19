using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Uml2Node.Generator.Extensions
{
    internal static class StringExtensions
    {
        internal static string ToStartCase(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }

        internal static string ToPascalCase(this string input)
        {
            string[] words = Regex.Split(input, @"(?<!^)(?=[A-Z])");

            return String.Join("", words.Select(e => e.ToStartCase()));
        }

        internal static string ToDashCase(this string input)
        {
            string[] words = Regex.Split(input, @"(?<!^)(?=[A-Z])");

            return String.Join("-", words.Select(e => e.ToLower()));
        }
    }
}
