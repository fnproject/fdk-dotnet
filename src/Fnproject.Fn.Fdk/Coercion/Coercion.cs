namespace Fnproject.Fn.Fdk.Coercion
{
    /// <summary>
    /// IInputCoercible can be implemented on an input type to change
    /// the deserialization logic for that type. 
    /// </summary>
    public interface IInputCoercible
    {
        /// <summary>
        /// Coerce gives the ability to deserialize an object from raw http body
        /// A user defined type can override this function to modify the
        /// deserialization logic.
        /// </summary>
        /// <param name="rawBody"><see cref="System.String"/> containing the raw request body</param>
        /// <returns>Object containering the deserialized object</returns>
        object Coerce(string rawBody);
    }

    /// <summary>
    /// IOutputCoercible can be implemented on an output type to change
    /// the serialization logic for that type. 
    /// </summary>
    public interface IOutputCoercible
    {
        /// <summary>
        /// Coerce gives the ability to serialize an object to a string.
        /// A user defined type can override this function to modify the
        /// serialization logic.
        /// </summary>
        /// <returns><see cref="System.String"/> containing the serialized object</returns>
        string Coerce();
    }
}
