namespace PCP.Application.Exceptions;

public class CourseNotFound : Exception
{
    public CourseNotFound() { }

    public CourseNotFound(string message)
        : base(message) { }

    public CourseNotFound(string message, System.Exception inner)
        : base(message, inner) { }
}