namespace BO;
using System.Reflection;

public static class Tools
{
    public static string ToStringProperty<T>(T t)
    {
        Type type = t.GetType();
        PropertyInfo[] properties = type.GetProperties();

        string result = $"{type.Name} properties:\n";

        foreach (PropertyInfo property in properties)
        {
            object? value = property.GetValue(t);
            if (value is IEnumerable<object> enumerableValue && !(value is string))
            {
                // If the property is an IEnumerable (excluding string), print all of its elements
                result += "[";

                foreach (var item in enumerableValue)
                {
                    result += $"{item}, ";
                }

                //if the result has ended with a comma, we need to remove it
                if (result.EndsWith(", ")) 
                {
                    result = result.Substring(0, result.Length - 2); //remove the trailing comma and space
                }

                result += "]";
            }
            else
            {
                //not an IEnumerable type, simply add the value to the result string
                result += $"{value}";
            }

            result += "\n";
        }
        return result;
    }
}
         
