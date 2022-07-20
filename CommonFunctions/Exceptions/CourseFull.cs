namespace PCP.Application.Exceptions;

public class CourseFull : Exception
{
    public CourseFull() { }

    public CourseFull(string message)
        : base(message) { }

    public CourseFull(string message, System.Exception inner)
        : base(message, inner) { }
}