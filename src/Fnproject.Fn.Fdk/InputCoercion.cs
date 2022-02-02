using System;
using System.Reflection;
using System.Text;

namespace Fnproject.Fn.Fdk {
  internal class InputCoercion 
  {
    private static object CoerceBuiltins(string input, Type t) {
        if(t == typeof(String)) {
          return (object) input;
        } else if (t == typeof(Byte[])) {
          return (object) Encoding.UTF8.GetBytes(input);
        } else {
          return (object) System.Text.Json.JsonSerializer.Deserialize(input, t);
        }
    }
    public static object Coerce(string input, Type t) {
      if (typeof(IInputCoercible).IsAssignableFrom(t)) {
        var instance = Activator.CreateInstance(t);
        MethodInfo method = t.GetMethod("Coerce");
        return method.Invoke(instance, new object[] { input });
      } else {
        return CoerceBuiltins(input, t);
      }
  }

  internal class OldInputCoercion<T> 
        where T : notnull
  {
    private static T CoerceBuiltins(string input) {
        var t = typeof(T);

        if(t == typeof(String)) {
          return (T) (object) input;
        } else if (t == typeof(Byte[])) {
          return (T) (object) Encoding.UTF8.GetBytes(input);
        } else {
          return System.Text.Json.JsonSerializer.Deserialize<T>(input);
        }
    }
    }
  }
}
