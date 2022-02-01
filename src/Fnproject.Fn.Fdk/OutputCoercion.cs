using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Fnproject.Fn.Fdk {
  internal class OutputCoercion<T> 
        where T : notnull
  {
    private static string coerceBuiltins(object input) {
        var t = input.GetType();

        if(t == typeof(String)) {
          return (string) (object) input;
        } else if (t == typeof(Byte[])) {
          return System.Text.Encoding.UTF8.GetString(input as byte[]);
        } else {
          return System.Text.Json.JsonSerializer.Serialize<T>((T) input);
        }
    }

    private static string coerce(object input) {
      if (typeof(IOutputCoercible).IsAssignableFrom(input.GetType())) {
        return ((IOutputCoercible) input).Coerce();
      } else {
        return coerceBuiltins(input);
      }
    }
    
    public static string Coerce(T input) {
      if(typeof(Task).IsAssignableFrom(input.GetType())) {
        Task task = input as Task;
        task.GetAwaiter().GetResult();
        var property = task.GetType().GetProperty("Result", BindingFlags.Public | BindingFlags.Instance);
        if (property == null) {
          throw new InvalidOperationException("Async functions should return a value (" + task.GetType().ToString() + ")");
        }
        return coerce(property.GetValue(task));
      } else {
        return coerce(input);
      }
    }

  }
}
