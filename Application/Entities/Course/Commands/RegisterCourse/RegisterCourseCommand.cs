using MediatR;

namespace PCP.Application.Entities.Course.Commands.RegisterCourse;

public class RegisterCourseCommand : IRequest<string>
{
    public long courseId { get; set; }
    public long UserId { get; set; }
    public DateTime startTime { get; set;  }
    public DateTime endTime { get; set;  }

}