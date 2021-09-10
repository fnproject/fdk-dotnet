using System;
using System.Reflection;
using System.Threading.Tasks;

using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("Fnproject.Fn.Fdk.Tests")]
namespace Fnproject.Fn.Fdk.Coercion
{
    internal class OutputCoercion
    {
        private static string coerceBuiltins(object input, Type t)
        {
            if (t == typeof(String))
            {
                return (string)(object)input;
            }
            else if (t == typeof(Byte[]))
            {
                return System.Text.Encoding.UTF8.GetString(input as byte[]);
            }
            else
            {
                return System.Text.Json.JsonSerializer.Serialize(input, t);
            }
        }

        internal static string Coerce(object input, Type t)
        {
            if (typeof(IOutputCoercible).IsAssignableFrom(t))
            {
                return ((IOutputCoercible)input).Coerce();
            }
            else
            {
                if (t != typeof(void)) return coerceBuiltins(input, t);
                else return "";
            }
        }
    }
}
