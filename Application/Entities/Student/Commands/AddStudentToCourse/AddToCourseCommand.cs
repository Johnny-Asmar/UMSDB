using MediatR;

namespace PCP.Application.Entities.Student.Commands.AddStudentToCourse;

public class AddToCourseCommand : IRequest<string>
{
    public long classId { get; set; }
    public long UserId { get; set; }
}