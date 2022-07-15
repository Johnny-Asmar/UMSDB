using AutoMapper;
using MediatR;
using NpgsqlTypes;
using PCP.Application.Exceptions;
using PCP.Application.ViewModel;
using Persistence;

namespace PCP.Application.Entities.AddCourseByAdmin.Commands.AddCourse;

public class AddCourseHandler : IRequestHandler<AddCourseCommand, CourseViewModel>
{
    public UmsContext _umsContext;
    private readonly IMapper _mapper;

    
    public AddCourseHandler(UmsContext umsContext, IMapper mapper)
    {
        _umsContext = umsContext;
        _mapper = mapper;

    }

    public async Task<CourseViewModel> Handle(AddCourseCommand request, CancellationToken cancellationToken)
    {
        Domain.Models.Course course = new Domain.Models.Course();
        course.Name = request.name;
        course.MaxStudentsNumber = request.maxNumbOfStudent;
        DateTime startDate = request.startDate;
        DateTime endDate = request.endDate;
        if (startDate >= endDate)
        {
            throw new StartDateHigher("Start Date Higher then End Date");
        }
        DateOnly startDateOnly = DateOnly.FromDateTime(startDate);
        DateOnly endDateOnly = DateOnly.FromDateTime(endDate);
        var Duration = new NpgsqlRange<DateOnly>(startDateOnly, endDateOnly);
        course.EnrolmentDateRange = Duration;
        _umsContext.Courses.AddAsync(course);
        _umsContext.SaveChanges();
        CourseViewModel courseViewModel = _mapper.Map<CourseViewModel>(course);
        return courseViewModel;
    }
    
}