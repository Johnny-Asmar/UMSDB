using MediatR;
using PCP.Application.ViewModel;

namespace PCP.Application.Entities.Course.Commands.CreateCourse;

public class AddCourseCommand : IRequest<CourseViewModel>
{
    public string name { get; set; }
    public int maxNumbOfStudent { get; set; }
    public DateTime startDate { get; set; }
    public DateTime endDate { get; set; }
    public long UserId { get; set; }

}