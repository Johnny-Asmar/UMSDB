using System.Net.Mail;
using System.Text;
using AutoMapper;
using CommonFunctions;
using CommonFunctions.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using PCP.Application.EmailObservablePattern.Classes;
using PCP.Application.EmailObservablePattern.Interfaces;
using PCP.Application.Exceptions;
using PCP.Application.ViewModel;
using Persistence;

namespace PCP.Application.Entities.Student.Commands.AddStudentToCourse;

public class AddToCourseHandler : IRequestHandler<AddToCourseCommand, string>
{
    public UmsContext _umsContext;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private ISendEmail _sendEmail;
    private ISubject _subject;

    public AddToCourseHandler(UmsContext umsContext, IMapper mapper, ILogger<AddToCourseHandler> logger, ISendEmail sendEmail, ISubject subject)
    {
        _umsContext = umsContext;
        _mapper = mapper;
        _logger = logger;
        _sendEmail = sendEmail;
        _subject = subject;
    }

    public async Task<string> Handle(AddToCourseCommand request, CancellationToken cancellationToken)
    {
        
        //check user Role
        long RoleId = (from u in _umsContext.Users
            where u.Id == request.UserId
            select u.RoleId).First();
        if (RoleId != 3)
        {
            throw new NotAuthorized("User Not Authorized to create course");
        }
        
        //check if class exists
        bool classExists = _umsContext.TeacherPerCourses.Any(x => x.Id == request.classId);
        if (classExists == false)
        {
            throw new ClassNotFound("Class Not Found");
        }
        bool classEnrolled = _umsContext.ClassEnrollments.Any(x => x.ClassId == request.classId 
                                                                   && x.StudentId == request.UserId);
        //check if already enrolled in class
        if (classEnrolled == true)
        {
            throw new CourseNotFound("Class already enrolled");
        }
        
        
        
        //getting course thru class id
            /*long classId = (from courseTable in _umsContext.Courses
                join TPCTable in _umsContext.TeacherPerCourses on courseTable.Id equals TPCTable.CourseId
                where courseTable.Id == request.courseId
                select TPCTable.Id).First();*/
            var courseId = (from TPCTable in _umsContext.TeacherPerCourses
                join courseTable in _umsContext.Courses on TPCTable.CourseId equals courseTable.Id
                where TPCTable.Id == request.classId
                select courseTable.Id).First();
            
            var courseMaxStudent = (from TPCTable in _umsContext.TeacherPerCourses
                join courseTable in _umsContext.Courses on TPCTable.CourseId equals courseTable.Id
                where TPCTable.Id == request.classId
                select courseTable.MaxStudentsNumber).First();
            
            var courseStartDate = (from TPCTable in _umsContext.TeacherPerCourses
                join courseTable in _umsContext.Courses on TPCTable.CourseId equals courseTable.Id
                where TPCTable.Id == request.classId
                select courseTable.EnrolmentDateRange.Value.LowerBound).First();
            
            var courseEndDate = (from TPCTable in _umsContext.TeacherPerCourses
                join courseTable in _umsContext.Courses on TPCTable.CourseId equals courseTable.Id
                where TPCTable.Id == request.classId
                select courseTable.EnrolmentDateRange.Value.UpperBound).First();
            
            //check if has exceeded the max number of Students
            
            int countStudents = (from userTable in _umsContext.Users join classTable in _umsContext.ClassEnrollments
                    on userTable.Id equals classTable.StudentId
                join TPCTable in _umsContext.TeacherPerCourses
                    on classTable.ClassId equals TPCTable.Id
                join courseTable in _umsContext.Courses on TPCTable.CourseId equals courseTable.Id
                where courseTable.Id == courseId
                select userTable).Count();
            
            if (countStudents >= courseMaxStudent)
            {
                throw new CourseFull("Course is Full");
            }
            
            
            //check if in date range
            
                DateTime currentDate = DateTime.Now;
                DateOnly lowerBound = courseStartDate;
                DateOnly upperBound = courseEndDate;
                DateTime lowerBoundDateTime = lowerBound.ToDateTime(TimeOnly.MinValue);
                DateTime upperBoundDateTime = upperBound.ToDateTime(TimeOnly.MinValue);
                if (currentDate < lowerBoundDateTime || currentDate > upperBoundDateTime)
                {
                    throw new OutOfDateRange("Out Of Date Range");
                }
                else //Insert Student in Course
                {
                    ClassEnrollment classEnrollment = new ClassEnrollment();
                    classEnrollment.StudentId = request.UserId;
                    classEnrollment.ClassId = request.classId;
                    _umsContext.ClassEnrollments.Add(classEnrollment);
                    _umsContext.SaveChanges();
                    
                    ClassViewModel classViewModel = _mapper.Map<ClassViewModel>(classEnrollment);
                    
                    
                    

                    string courseName = (from c in _umsContext.Courses
                        where c.Id == courseId
                        select c.Name).First();//get course name
                    
                   
                    
                    Domain.Models.User student = (from u in _umsContext.Users
                        where u.Id == request.UserId
                        select u).First(); //get user 
                    
                    string message = "You are enrolled to the course" + courseName + ".";
                    //push to elastic search
                    _logger.LogInformation(message);
                    //send email to student
                    _subject.SendEmailIfSubscribed(student, message);


                    return "Student successfuly enrolled";
                }
            
        }
        

    }
