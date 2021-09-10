namespace Fnproject.Fn.Fdk {
    public interface IInputCoercible<T> {
        T Coerce(string rawBody);
    }

    public interface IOutputCoercible<T> {
        string Coerce(T t);
    }
}