using Fnproject.Fn.Fdk;
using System.Threading;
using System;
 
namespace example
{
 
    class Input {
      public String message {get; set;}
 
      public Input() {
        message = "Hello Everyone";
      }
 
    }
 
    class Output {
      public String message {get; set;}
 
      public Output() {}
 
      public Output(String msg) {
        this.message = String.Format(msg);
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