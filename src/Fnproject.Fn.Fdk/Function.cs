using Fnproject.Fn.Fdk.Context;
using System;
using System.Reflection;
using System.Threading;

using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("Fnproject.Fn.Fdk.Tests")]
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
            public static IInvokeStrategy invokeStrategy;
            public static Type contextType;
            public static int dataArgPosition = Constants.UNASSIGNED_PARAM_INDEX;
            public static int ctxArgPosition = Constants.UNASSIGNED_PARAM_INDEX;
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
            return UserFunctionData.invokeStrategy.Invoke(args);
        }

        private static void assignContext(ParameterInfo parameter, int index)
        {
            UserFunctionData.ctxArgPosition = index;
            if (typeof(IRuntimeContext).IsAssignableFrom(parameter.ParameterType))
            {
                UserFunctionData.contextType = typeof(IRuntimeContext);
            }
            else
            {
                UserFunctionData.contextType = typeof(IHTTPContext);
            }
        }

        private static bool isContext(ParameterInfo parameter)
        {
            Type paramType = parameter.ParameterType;
            return typeof(IRuntimeContext).IsAssignableFrom(paramType) ||
                typeof(IHTTPContext).IsAssignableFrom(paramType);
        }

        private static void assignParameters(ParameterInfo[] parameters)
        {
            for (int index = 0; index < parameters.Length; index++)
            {
                ParameterInfo parameter = parameters[index];
                if (isContext(parameter))
                {
                    if (UserFunctionData.ctxArgPosition == Constants.UNASSIGNED_PARAM_INDEX)
                    {
                        assignContext(parameter, index);
                    }
                    else
                    {
                        throw new InvalidOperationException("Function can contain maximum 1 context");
                    }
                }
                else
                {
                    if (UserFunctionData.dataArgPosition == Constants.UNASSIGNED_PARAM_INDEX)
                    {
                        UserFunctionData.dataArgPosition = index;
                    }
                    else
                    {
                        throw new InvalidOperationException("Function can contain maximum 1 data argument");
                    }
                }
            }
        }

        internal static void Initialize(Type classType, MethodInfo method)
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
                UserFunctionData.invokeStrategy = InvokeStrategyFactory.Create(method, classType);

                var parameters = method.GetParameters();
                if (parameters.Length > Constants.MAX_ALLOWED_USER_FN_PARAMETERS)
                {
                    throw new ArgumentException("Function cannot contains more than 2 parameters");
                }
                else
                {
                    assignParameters(parameters);
                }
            }
            catch (Exception e)
            {
                rwl.ReleaseWriterLock();
                throw e;
            }
            rwl.ReleaseWriterLock();
        }
    }
}
