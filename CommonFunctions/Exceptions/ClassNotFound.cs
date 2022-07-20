namespace PCP.Application.Exceptions;

public class ClassNotFound : Exception
{
    public ClassNotFound() { }

    public ClassNotFound(string message)
        : base(message) { }

    public ClassNotFound(string message, System.Exception inner)
        : base(message, inner) { }
}