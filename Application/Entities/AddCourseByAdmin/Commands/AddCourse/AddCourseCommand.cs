using MediatR;
using PCP.Application.ViewModel;

namespace PCP.Application.Entities.AddCourseByAdmin.Commands.AddCourse;

public class AddCourseCommand : IRequest<CourseViewModel>
{
    public string name { get; set; }
    public int maxNumbOfStudent { get; set; }
    public DateTime startDate { get; set; }
    public DateTime endDate { get; set; }


}