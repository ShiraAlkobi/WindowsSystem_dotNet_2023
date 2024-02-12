namespace BO;
using System.Reflection;

/// <summary>
/// in this file we implement the ToString override function 
/// this function is generic and is used in all of the BL entities
/// </summary>
public static class Tools
{
    /// <summary>
    /// creates a string of the gotten object's properties and values
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <returns></returns>
    public static string ToStringProperty<T>(T t)
    {
        ///get the type of the gotten object, then create a propertyInfo array, for each property of the object T
        Type type = t.GetType(); 
        PropertyInfo[] properties = type.GetProperties();

        ///create the object's defining string 
        string result = $"\n{type.Name} properties:\n";

        foreach (PropertyInfo property in properties)
        {
            ///get the value for each property
            object? value = property.GetValue(t);

            if (value is not null)
            {

                ///if the property is an IEnumerable (excluding string), print all of its elements
                if (value is IEnumerable<object> enumerableValue && !(value is string))
                {
                    result += $"{property.Name} values: \n";
                    result += "[";

                    foreach (var item in enumerableValue)
                    {
                        result += $"{item}, ";
                    }

                    ///if the result has ended with a comma, we need to remove it
                    if (result.EndsWith(", "))
                    {
                        result = result.Substring(0, result.Length - 2); ///remove the trailing comma and space
                    }

                    result += "]";
                }
                else
                {
                    ///not an IEnumerable type, simply add the value to the result string
                    result += $"{property.Name} : {value}\n";
                }
                result += "\n";
            }
        }
        return result;
    }
}
         
