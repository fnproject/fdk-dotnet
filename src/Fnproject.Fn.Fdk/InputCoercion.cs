using System;
using System.Reflection;
using System.Text;

namespace Fnproject.Fn.Fdk {
  internal class InputCoercion<T> 
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
    public static T Coerce(string input) {
      if (typeof(IInputCoercible<T>).IsAssignableFrom(typeof(T))) {
        var inputType = typeof(T);
        var instance = Activator.CreateInstance(inputType);
        MethodInfo method = typeof(T).GetMethod("Coerce");
        return (T) method.Invoke(instance, new object[] { input });
      } else {
        return CoerceBuiltins(input);
      }
    }
  }
}
