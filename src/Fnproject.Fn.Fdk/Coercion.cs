namespace Fnproject.Fn.Fdk {
    public interface IInputCoercible<T> {
        T Coerce(string rawBody);
    }

    public interface IOutputCoercible {
        string Coerce();
    }
}

