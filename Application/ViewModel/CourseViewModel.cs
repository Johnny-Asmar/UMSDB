using NpgsqlTypes;

namespace PCP.Application.ViewModel;

public class CourseViewModel
{
    public string Name { get; set; }
    public int MaxStudentsNumber { get; set; }
    public DateOnly startTime { get; set; }
    public DateOnly endTime { get; set; }
}