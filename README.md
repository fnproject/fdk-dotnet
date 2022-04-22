# Fnproject.Fn.Fdk

This project contains an implementation of FDK for C# running on dotnet runtime.

## Usage
#### Add the project as a dependency from Nuget
https://www.nuget.org/packages/Fnproject.Fn.Fdk/

#### Use the Handle function in library to handle user function
```dotnet
using Fnproject.Fn.Fdk;

namespace example
{
    class Program
    {
        public static string userFunc(string input) {
          if(input.Length == 0) input = "World"
          return string.Format("Hello, {0}!", input);
        }

        static void Main(string[] args)
        {
          Fdk.Handle(arg[0]);
        }
    }
}
```
While caliing the function, pass `namespace:class:function` as arguments to the binary.
To run the above example, use `dotnet func.dll example:Program:userFunc`.

#### Handle other content types
The library exposes interfaces for users to write serialization and deserialization logic
- `IInputCoercible` - This interface can be implemented on user type to modify deserialization of request body.
- `IOutputCoercible` - This interface can be implemented on user type to modify serialization of response body.
