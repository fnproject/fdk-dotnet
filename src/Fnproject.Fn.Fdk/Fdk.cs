using System;
using System.Linq;
using System.Reflection;

namespace Fnproject.Fn.Fdk
{
    public class Fdk
    {
        const char TRIGGER_DELIMITER = ':';

        // Create an isvalid func
        private Fdk() { }

        private static bool invalidTrigger(string[] triggerSegments)
        {
            return triggerSegments.Length != 3 ||
              triggerSegments.Aggregate(false, (result, segment) => result |= segment.Length == 0);
        }
        public static void Handle(string trigger)
        {
            string[] triggerSegments = trigger.Split(TRIGGER_DELIMITER);
            if (invalidTrigger(triggerSegments))
            {
                throw new ArgumentException("Invalid trigger point: {0}", trigger);
            };
            string userNamespace = triggerSegments[0];
            string userClass = triggerSegments[1];
            string userNamespaceAndClass = string.Format("{0}.{1}", userNamespace, userClass);
            string userMethodName = triggerSegments[2];

            Type classType = null;

            AppDomain currentDomain = AppDomain.CurrentDomain;
            Assembly[] assems = currentDomain.GetAssemblies();
            foreach (Assembly asm in assems)
            {
                foreach (Type type in asm.GetTypes())
                {
                    if (type.Namespace == userNamespace && type.Name == userClass)
                        classType = type;
                }
            }

            if (classType == null)
                throw new ArgumentException("Class not found in loaded assemblies: {0}", userNamespaceAndClass);

            MethodInfo method = classType.GetMethod(userMethodName, BindingFlags.Static | BindingFlags.Public);
            if (method == null)
            {
                // No static method found in class, trying a non-static method
                method = classType.GetMethod(userMethodName);
            }
            if (method == null)
                throw new ArgumentException(string.Format("Method {1} not found in class {0}",
                    userNamespaceAndClass, userMethodName));

            Function.Initialize(classType, method);
            Server server = new Server();
            server.Run();
        }
    }
}
