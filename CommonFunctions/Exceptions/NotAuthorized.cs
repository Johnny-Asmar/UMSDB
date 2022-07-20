namespace PCP.Application.Exceptions;

public class NotAuthorized : Exception
{
    public NotAuthorized() { }

    public NotAuthorized(string message)
        : base(message) { }

    public NotAuthorized(string message, System.Exception inner)
        : base(message, inner) { }
}