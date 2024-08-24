using System.Text.RegularExpressions;

namespace FundingSouq.Assessment.Core.Extensions;

public static class Extensions
{
    public static bool IsEmpty<T>(this IEnumerable<T> collection)
    {
        // If T is string, use IsNullOrEmpty
        if (typeof(T) == typeof(char))
        {
            return string.IsNullOrEmpty(collection as string);
        }

        // Otherwise, check if collection has any elements
        return collection == null || !collection.Any();
    }

    public static bool IsNotEmpty<T>(this IEnumerable<T> collection)
    {
        // If T is string, use IsNullOrEmpty
        if (typeof(T) == typeof(char))
        {
            return !string.IsNullOrEmpty(collection as string);
        }

        // Otherwise, check if collection has any elements
        return collection != null && collection.Any();
    }

    public static string ToSnakeCase(this string str)
    {
        if (str.IsEmpty()) return string.Empty;

        str = str.Trim();

        // Replace spaces, hyphens, and other non-alphanumeric characters with an underscore
        str = Regex.Replace(str, @"[\s-]+", "_");

        // Convert to snake_case by adding an underscore before each uppercase letter 
        // (except the first letter if it’s uppercase)
        str = Regex.Replace(str, @"([a-z0-9])([A-Z])", "$1_$2");

        // Convert the entire string to lowercase
        str = str.ToLowerInvariant();

        return str;
    }
}