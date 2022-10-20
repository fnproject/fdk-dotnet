using System;
using Fnproject.Fn.Fdk;
using System.Threading;
namespace Function
{
    class Timeout
    {
        public string testTimeout(string input) {

          try{
              Thread.Sleep(6000);
              return string.Format("Hello World");
            }
          catch (TimeoutException e){
              return string.Format("{0}",e.Message);
          }
        }
        static void Main(string[] args)
        {
          Fdk.Handle(args[0]);
        }
    }
}
