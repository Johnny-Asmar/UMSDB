using AutoMapper;
using Domain.Models;
using MediatR;
using PCP.Application.Exceptions;
using Persistence;

namespace PCP.Application.Entities.RegisterCourseByTeacher.Commands.RegisterCourse;

public class RegisterCourseHandler : IRequestHandler<RegisterCourseCommand, string>
{
    public UmsContext _umsContext;
    

    public RegisterCourseHandler(UmsContext umsContext, IMapper mapper)
    {
        _umsContext = umsContext;
    }

    public async Task<string> Handle(RegisterCourseCommand request, CancellationToken cancellationToken)
    {
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
            teacherPerCourse.TeacherId = request.teacherId;
            teacherPerCourse.CourseId = course.Id;
            _umsContext.TeacherPerCourses.Add(teacherPerCourse);
            _umsContext.SaveChanges();

            //create a time slot
            
            SessionTime sessionTime = new SessionTime();
            sessionTime.StartTime = request.startTime.Date.ToLocalTime();
            sessionTime.EndTime = request.endTime.Date.ToLocalTime();
            TimeSpan durationDateTime = request.endTime - request.startTime;
            sessionTime.Duration = (int)durationDateTime.TotalHours;
            _umsContext.SessionTimes.Add(sessionTime);
            _umsContext.SaveChanges();

            //assign the course to the time slot

            TeacherPerCoursePerSessionTime teacherPerCoursePerSessionTime = new TeacherPerCoursePerSessionTime();
            teacherPerCoursePerSessionTime.TeacherPerCourseId = teacherPerCourse.Id;
            teacherPerCoursePerSessionTime.SessionTimeId = sessionTime.Id;
            _umsContext.TeacherPerCoursePerSessionTimes.Add(teacherPerCoursePerSessionTime);

            
            

            
            _umsContext.SaveChanges();

            return "Added succesfuly";
        }
    }
}