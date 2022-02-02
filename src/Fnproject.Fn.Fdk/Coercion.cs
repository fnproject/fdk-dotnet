namespace Fnproject.Fn.Fdk {
    public interface IInputCoercible {
        object Coerce(string rawBody);
    }

    public interface IOutputCoercible {
        string Coerce();
    }
}

