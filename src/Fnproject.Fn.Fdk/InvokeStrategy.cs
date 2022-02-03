using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Fnproject.Fn.Fdk {
  internal interface IInvokeStrategy {
    object Invoke(object[] args);
  }

  internal class InvokeStatic: IInvokeStrategy {
      private readonly MethodInfo method;
      public object Invoke(object[] args) {
        return method.Invoke(null, args);
      }
      public InvokeStatic(MethodInfo method) {
        this.method = method;
      }
    }

    internal class InvokeNonStaticAsync: IInvokeStrategy {
      private readonly MethodInfo method;
      private readonly object instance;
      public object Invoke(object[] args) {
        Task task = (Task) method.Invoke(instance, args);
        task.GetAwaiter().GetResult();
        var resultProperty = task.GetType().GetProperty("Result");
        return resultProperty.GetValue(task);
      }
      public InvokeNonStaticAsync(MethodInfo method, Type classType) {
        this.method = method;
        instance = Activator.CreateInstance(classType);
      }
    }

    internal class InvokeNonStatic: IInvokeStrategy {
      private readonly MethodInfo method;
      private readonly object instance;
      public object Invoke(object[] args) {
        return method.Invoke(instance, args);
      }
      public InvokeNonStatic(MethodInfo method, Type classType) {
        this.method = method;
        instance = Activator.CreateInstance(classType);
      }
    }

    internal class InvokeStaticAsync: IInvokeStrategy {
      private readonly MethodInfo method;
      public object Invoke(object[] args) {
        Task task = (Task) method.Invoke(null, args);
        task.GetAwaiter().GetResult();
        var resultProperty = task.GetType().GetProperty("Result");
        return resultProperty.GetValue(task);
      }
      public InvokeStaticAsync(MethodInfo method) {
        this.method = method;
      }
    }
}