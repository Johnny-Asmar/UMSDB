namespace PCP.Application.Exceptions;

public class OutOfDateRange : Exception
{
    public OutOfDateRange() { }

    public OutOfDateRange(string message)
        : base(message) { }

    public OutOfDateRange(string message, System.Exception inner)
        : base(message, inner) { }
}