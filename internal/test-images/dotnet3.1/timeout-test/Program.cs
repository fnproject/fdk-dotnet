using Fnproject.Fn.Fdk;
using System.Threading;
using System;
 
namespace example
{
 
    class Input {
      public String message {get; set;}
 
      public Input() {
        message = "Hello World";
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
          try{
              Thread.Sleep(7000);
              return new Output(input.message);
            }
          catch (TimeoutException e){
            String msg="Timed out";
            return new Output(msg);
          }
 
        }
 
        static void Main(string[] args)
        {
          Fdk.Handle<Input, Output>(Program.userFunc);
        }
    }
}