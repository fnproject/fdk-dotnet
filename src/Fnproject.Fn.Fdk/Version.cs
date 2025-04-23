namespace Fnproject.Fn.Fdk
{
    sealed class Version
    {
        private static readonly string version = "1.0.44";

        private Version() { }

        public static string Value
        {
            get { return version; }
            private set {}
        }
    }
}
