using MediatR;

namespace PCP.Application.Entities.RegisterCourseByTeacher.Commands.RegisterCourse;

public class RegisterCourseCommand : IRequest<string>
{
    public long courseId { get; set; }
    public long teacherId { get; set; }
    public DateTime startTime { get; set;  }
    public DateTime endTime { get; set;  }

}