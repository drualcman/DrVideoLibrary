using System.Reflection;

namespace DrVideoLibrary.Cosmos.DbContext.Helpers;
internal static class ObjectConverter
{
    public static object ConvertToLowercaseObject<T>(T model, string registerValue)
    {
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var dictionary = properties
            .ToDictionary(
                prop => prop.Name.ToLower(),
                prop => prop.GetValue(model)
            );
        dictionary["register"] = registerValue;

        return dictionary;
    }
}
