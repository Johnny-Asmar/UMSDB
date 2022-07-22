using AutoMapper;
using CommonFunctions;
using CommonFunctions.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using NpgsqlTypes;
using PCP.Application.EmailObservablePattern.Classes;
using PCP.Application.EmailObservablePattern.Interfaces;
using PCP.Application.Exceptions;
using PCP.Application.ViewModel;
using Persistence;

namespace PCP.Application.Entities.Course.Commands.CreateCourse;

public class AddCourseHandler : IRequestHandler<AddCourseCommand, CourseViewModel>
{
    public UmsContext _umsContext;
    private readonly IMapper _mapper;
    private ISubject _subject;
    private ILogger _logger;
    
    public AddCourseHandler(UmsContext umsContext, IMapper mapper, ISubject subject, ILogger<AddCourseHandler> logger)
    {
        _umsContext = umsContext;
        _mapper = mapper;
        _subject = subject;
        _logger = logger;
    }

    public async Task<CourseViewModel> Handle(AddCourseCommand request, CancellationToken cancellationToken)
    {
        //check user Role
        long RoleId = (from u in _umsContext.Users
            where u.Id == request.UserId
            select u.RoleId).First();
        if (RoleId != 1)
        {
            throw new NotAuthorized("User Not Authorized to create course");
        }
        Domain.Models.Course course = new Domain.Models.Course();
        course.Name = request.name;
        course.MaxStudentsNumber = request.maxNumbOfStudent;
        DateTime startDate = request.startDate;
        DateTime endDate = request.endDate;
        if (request.maxNumbOfStudent <= 0)
        {
            throw new MaxStudentNumberNotCompatible("Max Student is not an applicant number");
        }
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
        
        
        string message = "Please be aware that course " + course.Name + " is added to the courses list.";

        //inform students of the course
       
        _logger.LogInformation(message);
        _subject.RefreshObservers(); //Filtering observers subscribed to Email
        _subject.notifyObservers(message);
        
        return courseViewModel;
    }
    
}