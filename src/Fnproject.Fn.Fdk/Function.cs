using System; 
using System.Reflection; 
using System.Threading; 
using System.Threading.Tasks; 

namespace Fnproject.Fn.Fdk {
  internal interface IInvokeStrategy {
    object Invoke(object[] args);
  }

  sealed internal class Function {
    private class InvokeStatic: IInvokeStrategy {
      private readonly MethodInfo method;
      public object Invoke(object[] args) {
        return method.Invoke(null, args);
      }
      public InvokeStatic(MethodInfo method) {
        this.method = method;
      }
    }

    private class InvokeNonStaticAsync: IInvokeStrategy {
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

    private class InvokeNonStatic: IInvokeStrategy {
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

    private class InvokeStaticAsync: IInvokeStrategy {
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

    private class UserFunctionData {
      public static Type classType;
      public static MethodInfo method;
      public static IInvokeStrategy strategy;
      public static int dataArgPosition = 0;
      public static int ctxArgPosition = 1;
      private UserFunctionData() {}
    }

    private static ReaderWriterLock rwl = new ReaderWriterLock();

    public static int ContextParameterIndex {
      get {
        int index = -1;
        rwl.AcquireReaderLock(Timeout.Infinite);
        index = UserFunctionData.ctxArgPosition;
        rwl.ReleaseReaderLock();
        return index;
      }
      private set{}
    }

    public static int DataParameterIndex {
      get {
        int index = -1;
        rwl.AcquireReaderLock(Timeout.Infinite);
        index = UserFunctionData.dataArgPosition;
        rwl.ReleaseReaderLock();
        return index;
      }
      private set{}
    }

    public static Type ClassType {
      get {
        Type t;
        rwl.AcquireReaderLock(Timeout.Infinite);
        t = UserFunctionData.classType;
        rwl.ReleaseReaderLock();
        return t;
      }
      private set{}
    }

    public static MethodInfo Method {
      get {
        MethodInfo m;
        rwl.AcquireReaderLock(Timeout.Infinite);
        m = UserFunctionData.method;
        rwl.ReleaseReaderLock();
        return m;
      }
      private set{}
    }

    private Function() {}

    public static object Invoke(object[] args) {
      try {
        return UserFunctionData.strategy.Invoke(args);
      } catch (Exception e) {
        Console.WriteLine("Failed to invoke: {0}", e);
        return null;
      }
    }

    public static void Initialize(Type classType, MethodInfo method) {
      rwl.AcquireWriterLock(Timeout.Infinite);
      try {
        if(UserFunctionData.classType != null || UserFunctionData.method != null) {
          throw new InvalidOperationException("Value of class type or method already set");
        } 

        UserFunctionData.classType = classType;
        UserFunctionData.method = method;

        // Check static
        if(method.IsStatic) {
          if(typeof(Task).IsAssignableFrom(method.ReturnType)) {
            UserFunctionData.strategy = new InvokeStaticAsync(method);
          } else {
            UserFunctionData.strategy = new InvokeStatic(method);
          }
        } else {
          if(typeof(Task).IsAssignableFrom(method.ReturnType)) {
            UserFunctionData.strategy = new InvokeNonStaticAsync(method, classType);
          } else {
            UserFunctionData.strategy = new InvokeNonStatic(method, classType);
          }
        }

        // Evaluate Argument Indices
        var parameters = method.GetParameters();
        if(parameters.Length > 2) throw new ArgumentException("User function should contain an input and an optional context");

        UserFunctionData.ctxArgPosition = -1;
        UserFunctionData.dataArgPosition = -1;

        switch (parameters.Length) {
         case 0:
           UserFunctionData.ctxArgPosition = -1;
           UserFunctionData.dataArgPosition = -1;
           break;
         case 1:
           if(typeof(IContext).IsAssignableFrom(parameters[0].ParameterType)) {
             UserFunctionData.ctxArgPosition = 0;
           } else {
             UserFunctionData.dataArgPosition = 0;
           }
           break;
         default:
           int ctxArgCount = 0;
           int nonCtxArgCount = 0;

           if(typeof(IContext).IsAssignableFrom(parameters[0].ParameterType)) {
             ctxArgCount++;
             UserFunctionData.ctxArgPosition = 0;
           } else {
             nonCtxArgCount++;
             UserFunctionData.dataArgPosition = 0;
           }

           if(typeof(IContext).IsAssignableFrom(parameters[1].GetType())) {
             ctxArgCount++;
             UserFunctionData.ctxArgPosition = 1;
           } else {
             nonCtxArgCount++;
             UserFunctionData.dataArgPosition = 1;
           }

           if(ctxArgCount != 1 || nonCtxArgCount != 1) {
             throw new InvalidOperationException("Function can contain maximum 1 context and data argument");
           }
           break;
        }
      } catch (Exception e) {
        Console.WriteLine(e);
      } finally {
        rwl.ReleaseWriterLock();
      }
    }
  }
}
