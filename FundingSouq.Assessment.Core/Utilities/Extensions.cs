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
}