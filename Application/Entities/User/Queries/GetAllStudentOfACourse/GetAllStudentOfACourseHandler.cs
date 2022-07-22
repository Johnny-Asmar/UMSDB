using AutoMapper;
using MediatR;
using PCP.Application.Entities.User.Queries.GetAllUsers;
using PCP.Application.ViewModel;
using Persistence;

namespace PCP.Application.Entities.User.Queries.GetAllStudentOfACourse;

public class GetAllStudentOfACourseHandler: IRequestHandler<GetAllStudentsOfACourseQuery, List<UserViewModel>>
{
    private UmsContext _umsContext ;
    private IMapper _mapper;
    
    public GetAllStudentOfACourseHandler(UmsContext umsContext, IMapper mapper)
    {
        _umsContext = umsContext;
        _mapper = mapper;
    }

   
    public async Task<List<UserViewModel>> Handle(GetAllStudentsOfACourseQuery request, CancellationToken cancellationToken)
    {
        List<Domain.Models.User> users = (from userTable in _umsContext.Users
            join classTable in _umsContext.ClassEnrollments
                on userTable.Id equals classTable.StudentId
            join TPCTable in _umsContext.TeacherPerCourses
                on classTable.ClassId equals TPCTable.Id
            join courseTable in _umsContext.Courses on TPCTable.CourseId equals courseTable.Id
            where courseTable.Id == request.courseId
            select userTable).ToList();
        List<UserViewModel> usersViewModel = _mapper.Map<List<UserViewModel>>(users);
        return usersViewModel;
    }
}