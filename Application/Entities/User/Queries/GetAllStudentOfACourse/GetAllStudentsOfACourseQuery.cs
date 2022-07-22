using MediatR;
using PCP.Application.ViewModel;

namespace PCP.Application.Entities.User.Queries.GetAllStudentOfACourse;

public class GetAllStudentsOfACourseQuery: IRequest<List<UserViewModel>>
{
    public int courseId { get; set; }
}