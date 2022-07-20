
using CommonFunctions;
using CommonFunctions.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using PCP.Application.Exceptions;
using Persistence;

namespace PCP.Application.Entities.Course.Commands.RegisterCourse;

public class RegisterCourseHandler : IRequestHandler<RegisterCourseCommand, string>
{
    public UmsContext _umsContext;
    private readonly ILogger _logger;
    private ISendEmail _sendEmail;

    public RegisterCourseHandler(UmsContext umsContext, ILogger<RegisterCourseHandler> logger, ISendEmail sendEmail)
    {
        _umsContext = umsContext;
        _logger = logger;
        _sendEmail = sendEmail;
    }

    public async Task<string> Handle(RegisterCourseCommand request, CancellationToken cancellationToken)
    {
        //check user Role
        long RoleId = (from u in _umsContext.Users
            where u.Id == request.UserId
            select u.RoleId).First();
        if (RoleId != 2)
        {
            throw new NotAuthorized("User Not Authorized to create course");
        }
        
        //check if course exists
        bool courseExists = _umsContext.Courses.Any(x => x.Id == request.courseId);
        if (courseExists == false)
        {
            return "Course Not Found";
        }
        else if (request.startTime >= request.endTime)
        {
            throw new StartDateHigher("Start Date Higher then End Date");
        }
        else
        {
            //get course
            Domain.Models.Course course = (from c in _umsContext.Courses
                where c.Id == request.courseId
                select c).First();
            
            //teacher register the course
            
            TeacherPerCourse teacherPerCourse = new TeacherPerCourse();
            teacherPerCourse.TeacherId = request.UserId;
            teacherPerCourse.CourseId = course.Id;
            _umsContext.TeacherPerCourses.Add(teacherPerCourse);
            _umsContext.SaveChanges();

            //create a time slot
            
            SessionTime sessionTime = new SessionTime();
            sessionTime.StartTime = request.startTime.Date.ToLocalTime();
            sessionTime.EndTime = request.endTime.Date.ToLocalTime();
            _umsContext.SessionTimes.Add(sessionTime);
            _umsContext.SaveChanges();

            //assign the course to the time slot

            TeacherPerCoursePerSessionTime teacherPerCoursePerSessionTime = new TeacherPerCoursePerSessionTime();
            teacherPerCoursePerSessionTime.TeacherPerCourseId = teacherPerCourse.Id;
            teacherPerCoursePerSessionTime.SessionTimeId = sessionTime.Id;
            _umsContext.TeacherPerCoursePerSessionTimes.Add(teacherPerCoursePerSessionTime);

            
            

            
            _umsContext.SaveChanges();

            string message = "Please be aware that a new class schedule have been added to the course " + course.Name;
            //inform students of the class
            List<Domain.Models.User> users = _umsContext.Users.Select(x => x).ToList();
            foreach (var user in users)
            {
                _logger.LogInformation(message);
                _sendEmail.sendEmailToStudent(user.Email, message);
            }

            return "Added succesfuly";
        }
    }
}