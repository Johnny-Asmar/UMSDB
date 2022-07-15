using MediatR;

namespace PCP.Application.Entities.EnrollCourseByStudent.Commands.AddToCourse;

public class AddToCourseCommand : IRequest<string>
{
    public long courseId { get; set; }
    public long studentId { get; set;  }
    
}