using AutoMapper;
using CommonFunctions;
using CommonFunctions.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using NpgsqlTypes;
using PCP.Application.Exceptions;
using PCP.Application.ViewModel;
using Persistence;

namespace PCP.Application.Entities.Course.Commands.CreateCourse;

public class AddCourseHandler : IRequestHandler<AddCourseCommand, CourseViewModel>
{
    public UmsContext _umsContext;
    private readonly IMapper _mapper;
    private ISendEmail _sendEmail;
    private readonly ILogger _logger;
    
    public AddCourseHandler(UmsContext umsContext, IMapper mapper, ISendEmail sendEmail, ILogger<AddCourseHandler> logger)
    {
        _umsContext = umsContext;
        _mapper = mapper;
        _sendEmail = sendEmail;
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
        List<Domain.Models.User> users = _umsContext.Users.Select(x => x).ToList();
        foreach (var user in users)
        {
            _logger.LogInformation(message);
            _sendEmail.sendEmailToStudent(user.Email, message);
        }
        
        return courseViewModel;
    }
    
}