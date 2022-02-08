using System;
using System.Reflection;
using System.Text;

using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("Fnproject.Fn.Fdk.Tests")]
namespace Fnproject.Fn.Fdk.Coercion
{
    internal class InputCoercion
    {
        private static object CoerceBuiltins(string input, Type t)
        {
            if (t == typeof(String))
            {
                return (object)input;
            }
            else if (t == typeof(Byte[]))
            {
                return (object)Encoding.UTF8.GetBytes(input);
            }
            else
            {
                return (object)System.Text.Json.JsonSerializer.Deserialize(input, t);
            }
        }
        internal static object Coerce(string input, Type t)
        {
            if (typeof(IInputCoercible).IsAssignableFrom(t))
            {
                var instance = Activator.CreateInstance(t);
                MethodInfo method = t.GetMethod(Constants.COERCION_INTERFACE_METHOD);
                return method.Invoke(instance, new object[] { input });
            }
            else
            {
                return CoerceBuiltins(input, t);
            }
        }
    }
}
