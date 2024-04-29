using System;
using Fnproject.Fn.Fdk;

namespace Function
{
    class Input
    {
        public string name { get; set; }
    }

    class Output
    {
        public string message { get; set; }
    }
    class Greeter
    {
        public Output helloWorld(Input input)
        {
            Output res = new Output();
            res.message = string.Format("Hello {0}", input != null && input.name.Length == 0
                ? "World" : input.name.Trim());
            return res;
        }

        static void Main(string[] args)
        {
            Fdk.Handle(args[0]);
        }
    }
}
