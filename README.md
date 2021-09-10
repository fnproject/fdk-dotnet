# Fnproject.Fn.Fdk

This project contains an implementation of FDK for C# running on dotnet runtime.

## Usage
#### Add the project as a dependency from Nuget
https://www.nuget.org/packages/Fnproject.Fn.Fdk/

#### Use the Handle function in library to handle user function
```csharp
using System;
using Fnproject.Fn.Fdk;

namespace example
{

    class Input {
      public String message {get; set;}

      public Input() {
        message = "World";
      }
    }

    class Output {
      public String greeting {get; set;}

      public Output() {}

      public Output(String msg) {
        this.greeting = String.Format("Hello {0}!", msg);
      }
    }

    class Program
    {
        static Func<IContext, Input, Output> userFunc = (ctx, input) => {
          return new Output(input.message);
        };

        static void Main(string[] args)
        {
          Fdk.Handle(Program.userFunc);
        }
    }
}
```
##### Alternatively users can use a static function from class
```csharp
using System;
using Fnproject.Fn.Fdk;

namespace example
{

    class Input {
      public String message {get; set;}

      public Input() {
        message = "World";
      }
    }

    class Output {
      public String greeting {get; set;}

      public Output() {}

      public Output(String msg) {
        this.greeting = String.Format("Hello {0}!", msg);
      }
    }

    class Program
    {
        public static Output userFunc(IContext ctx, Input input) {
          return new Output(input.message);
        }

        static void Main(string[] args)
        {
          Fdk.Handle<Input, Output>(Program.userFunc);
        }
    }
}
```

#### Handle other content types
The library exposes interfaces for users to write serialization and deserialization logic
- `IInputCoercible<T>` - This interface can be implemented on user type to modify deserialization of request body.
- `IOutputCoercible<T>` - This interface can be implemented on user type to modify serialization of response body.
