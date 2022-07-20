using Persistence;

namespace CommonFunctions;

public class CheckCourseExists
{
    
    public UmsContext _umsContext;

    public CheckCourseExists(UmsContext umsContext)
    {
        _umsContext = umsContext;
    }

    public bool CourseExists(long courseId)
    {
        bool courseExists = _umsContext.Courses.Any(x => x.Id == courseId);
        return courseExists;

    }

}