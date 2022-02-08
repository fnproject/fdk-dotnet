using System;
using System.Linq;
using System.Reflection;

namespace Fnproject.Fn.Fdk
{
    /// <summary>
    /// Fdk is the entrypoint in the code. Fdk provides Handle() API
    /// which helps running the function.
    /// </summary>
    public class Fdk
    {
        private Fdk() { }

        private static bool invalidTrigger(string[] triggerSegments)
        {
            return triggerSegments.Length != Constants.NUMBER_OF_TRIGGER_SEGMENTS ||
              triggerSegments.Aggregate(false,
                (result, segment) => result |= string.IsNullOrEmpty(segment)
              );
        }

        /// <summary>
        /// Handle() is the entrypoint into the FDK. Handle() expects a trigger 
        /// point in the form "namespace:class:function", this trigger will be 
        /// executed upon running the function in cloud.
        /// </summary>
        /// <param name="trigger">
        /// Trigger should be in the form "namespace:class:function"
        /// </param>
        public static void Handle(string trigger)
        {
            string[] triggerSegments = trigger.Split(Constants.TRIGGER_DELIMITER);
            if (invalidTrigger(triggerSegments))
            {
                throw new ArgumentException("Invalid trigger point: {0}", trigger);
            };
            string userNamespace = triggerSegments[0];
            string userClass = triggerSegments[1];
            string userNamespaceAndClass = string.Format("{0}.{1}", userNamespace, userClass);
            string userMethodName = triggerSegments[2];

            Type classType = Assembly.GetCallingAssembly().GetType(userNamespaceAndClass, true);

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
