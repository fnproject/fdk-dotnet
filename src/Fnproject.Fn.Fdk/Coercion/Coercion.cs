namespace Fnproject.Fn.Fdk.Coercion
{
    public interface IInputCoercible
    {
        object Coerce(string rawBody);
    }

    public interface IOutputCoercible
    {
        string Coerce();
    }
}

