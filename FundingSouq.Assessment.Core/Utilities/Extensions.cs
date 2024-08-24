using System.Text.RegularExpressions;

namespace FundingSouq.Assessment.Core.Extensions;

/// <summary>
/// Provides extension methods for common operations on collections and strings.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Checks if a collection is empty or null.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to check.</param>
    /// <returns><c>true</c> if the collection is null or has no elements; otherwise, <c>false</c>.</returns>
    public static bool IsEmpty<T>(this IEnumerable<T> collection)
    {
        // Handle string type separately using IsNullOrEmpty
        if (typeof(T) == typeof(char))
        {
            return string.IsNullOrEmpty(collection as string);
        }

        // Check if collection is null or has no elements
        return collection == null || !collection.Any();
    }

    /// <summary>
    /// Checks if a collection is not empty and not null.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to check.</param>
    /// <returns><c>true</c> if the collection is not null and has elements; otherwise, <c>false</c>.</returns>
    public static bool IsNotEmpty<T>(this IEnumerable<T> collection)
    {
        // Handle string type separately using IsNullOrEmpty
        if (typeof(T) == typeof(char))
        {
            return !string.IsNullOrEmpty(collection as string);
        }

        // Check if collection has elements
        return collection != null && collection.Any();
    }

    /// <summary>
    /// Converts a string to snake_case format.
    /// </summary>
    /// <param name="str">The string to convert.</param>
    /// <returns>The snake_case version of the string.</returns>
    public static string ToSnakeCase(this string str)
    {
        // Return empty string if input is null or empty
        if (str.IsEmpty()) return string.Empty;

        str = str.Trim();

        // Replace spaces, hyphens, and other non-alphanumeric characters with an underscore
        str = Regex.Replace(str, @"[\s-]+", "_");

        // Add an underscore before each uppercase letter that follows a lowercase letter or digit
        str = Regex.Replace(str, @"([a-z0-9])([A-Z])", "$1_$2");

        // Convert the entire string to lowercase
        return str.ToLowerInvariant();
    }
}