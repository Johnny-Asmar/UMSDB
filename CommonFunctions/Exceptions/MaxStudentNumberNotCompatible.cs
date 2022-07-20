namespace PCP.Application.Exceptions;

public class MaxStudentNumberNotCompatible : Exception
{
    public MaxStudentNumberNotCompatible() { }

    public MaxStudentNumberNotCompatible(string message)
        : base(message) { }

    public MaxStudentNumberNotCompatible(string message, System.Exception inner)
        : base(message, inner) { }
}