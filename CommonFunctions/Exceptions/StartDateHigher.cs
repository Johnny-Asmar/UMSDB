namespace PCP.Application.Exceptions;

public class StartDateHigher : Exception
{
    public StartDateHigher() { }

    public StartDateHigher(string message)
        : base(message) { }

    public StartDateHigher(string message, System.Exception inner)
        : base(message, inner) { }
}