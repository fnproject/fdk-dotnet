using Fnproject.Fn.Fdk.Context;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Fnproject.Fn.Fdk
{
    // Function class which contains the metadata of the function.
    // This class has a private constuctor and static members can
    // initialized once.
    sealed internal class Function
    {
        private class UserFunctionData
        {
            public static Type classType;
            public static MethodInfo method;
            public static IInvokeStrategy strategy;
            public static Type contextType;
            public static int dataArgPosition = -1;
            public static int ctxArgPosition = -1;
            private UserFunctionData() { }
        }

        private static ReaderWriterLock rwl = new ReaderWriterLock();

        public static int ContextParameterIndex
        {
            get
            {
                int index = -1;
                rwl.AcquireReaderLock(Timeout.Infinite);
                index = UserFunctionData.ctxArgPosition;
                rwl.ReleaseReaderLock();
                return index;
            }
            private set { }
        }

        public static int DataParameterIndex
        {
            get
            {
                int index = -1;
                rwl.AcquireReaderLock(Timeout.Infinite);
                index = UserFunctionData.dataArgPosition;
                rwl.ReleaseReaderLock();
                return index;
            }
            private set { }
        }

        public static Type ClassType
        {
            get
            {
                Type t;
                rwl.AcquireReaderLock(Timeout.Infinite);
                t = UserFunctionData.classType;
                rwl.ReleaseReaderLock();
                return t;
            }
            private set { }
        }

        public static MethodInfo Method
        {
            get
            {
                MethodInfo m;
                rwl.AcquireReaderLock(Timeout.Infinite);
                m = UserFunctionData.method;
                rwl.ReleaseReaderLock();
                return m;
            }
            private set { }
        }

        public static Type ContextType
        {
            get
            {
                Type t;
                rwl.AcquireReaderLock(Timeout.Infinite);
                t = UserFunctionData.contextType;
                rwl.ReleaseReaderLock();
                return t;
            }
            private set { }
        }

        private Function() { }

        public static object Invoke(object[] args)
        {
            try
            {
                return UserFunctionData.strategy.Invoke(args);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to invoke: {0}", e);
                return null;
            }
        }

        private static void assignParam(ParameterInfo parameter, int index,
              ref int ctxArgCount, ref int nonCtxArgCount) {
          if (typeof(IRuntimeContext).IsAssignableFrom(parameter.ParameterType))
            {
                ctxArgCount++;
                UserFunctionData.ctxArgPosition = index;
                UserFunctionData.contextType = typeof(IRuntimeContext);
            }
            else if (typeof(IHTTPContext).IsAssignableFrom(parameter.ParameterType))
            {
                ctxArgCount++;
                UserFunctionData.ctxArgPosition = index;
                UserFunctionData.contextType = typeof(IHTTPContext);
            }
            else
            {
                nonCtxArgCount++;
                UserFunctionData.dataArgPosition = index;
            }
        }
        private static void assignParameters(ParameterInfo[] parameters)
        {
            int ctxArgCount = 0;
            int nonCtxArgCount = 0;
            
            for(int index=0; index<parameters.Length; index++) {
              assignParam(parameters[index], index, ref ctxArgCount, ref nonCtxArgCount);
            }

            if (ctxArgCount == 2 || nonCtxArgCount == 2)
            {
                throw new InvalidOperationException("Function can contain maximum 1 context and data argument");
            }
        }
        public static void Initialize(Type classType, MethodInfo method)
        {
            rwl.AcquireWriterLock(Timeout.Infinite);
            try
            {
                if (UserFunctionData.classType != null || UserFunctionData.method != null)
                {
                    throw new InvalidOperationException("Value of class type or method already set");
                }

                UserFunctionData.classType = classType;
                UserFunctionData.method = method;

                // Check static
                if (method.IsStatic)
                {
                    if (typeof(Task).IsAssignableFrom(method.ReturnType))
                    {
                        UserFunctionData.strategy = new InvokeStaticAsync(method);
                    }
                    else
                    {
                        UserFunctionData.strategy = new InvokeStatic(method);
                    }
                }
                else
                {
                    if (typeof(Task).IsAssignableFrom(method.ReturnType))
                    {
                        UserFunctionData.strategy = new InvokeNonStaticAsync(method, classType);
                    }
                    else
                    {
                        UserFunctionData.strategy = new InvokeNonStatic(method, classType);
                    }
                }

                // Evaluate Argument Indices
                var parameters = method.GetParameters();
                if (parameters.Length > 2) throw new ArgumentException("User function should contain an input and an optional context");
                
                switch (parameters.Length)
                {
                    case 0:
                        UserFunctionData.ctxArgPosition = -1;
                        UserFunctionData.dataArgPosition = -1;
                        break;
                    case 1:
                        assignParameters(parameters);
                        break;
                    default:
                        assignParameters(parameters);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                rwl.ReleaseWriterLock();
            }
        }
    }
}
