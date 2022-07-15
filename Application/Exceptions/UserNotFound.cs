namespace PCP.Application.Exceptions;

public class UserNotFound : System.Exception
{
    public UserNotFound() { }

    public UserNotFound(string message)
        : base(message) { }

    public UserNotFound(string message, System.Exception inner)
        : base(message, inner) { }
}