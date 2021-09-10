namespace Fnproject.Fn.Fdk
{
    sealed class Version
    {
        private static string version = "0.0.1";

        private Version() { }

        public static string Get()
        {
            return Version.version;
        }
    }
}
