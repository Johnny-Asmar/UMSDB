using System.Net.Mail;
using System.Text;
using AutoMapper;
using Domain.Models;
using MediatR;
using PCP.Application.Exceptions;
using PCP.Application.ViewModel;
using Persistence;
using System.Net;    
using System.Net.Mail; 

namespace PCP.Application.Entities.EnrollCourseByStudent.Commands.AddToCourse;

public class AddToCourseHandler : IRequestHandler<AddToCourseCommand, string>
{
    public UmsContext _umsContext;
    
    private readonly IMapper _mapper;

    public AddToCourseHandler(UmsContext umsContext, IMapper mapper)
    {
        _umsContext = umsContext;
        _mapper = mapper;
    }

    public async Task<string> Handle(AddToCourseCommand request, CancellationToken cancellationToken)
    {
        bool courseExists = _umsContext.Courses.Any(x => x.Id == request.courseId);
        //check if course exist
        if (courseExists == false)
        {
            throw new CourseNotFound("Course not found");
        }
        
        else //if not null
            
        {
            //getting class id thru course id
            long classId = (from courseTable in _umsContext.Courses
                join TPCTable in _umsContext.TeacherPerCourses on courseTable.Id equals TPCTable.CourseId
                where courseTable.Id == request.courseId
                select TPCTable.Id).First();
            
            //check if already enrolled
            bool alreadyEnrolled = _umsContext.ClassEnrollments.Any(x => x.ClassId == classId 
                                                                         && x.StudentId == request.studentId);
            if (alreadyEnrolled == true)
            {
                return "student already enrolled";
            }
            else
            {
                
            }
            //get course
            Domain.Models.Course course = (from c in _umsContext.Courses
                where c.Id == request.courseId
                select c).First();
            
            //check if has exceeded the max number of Students
            
            int countStudents = (from userTable in _umsContext.Users join classTable in _umsContext.ClassEnrollments
                    on userTable.Id equals classTable.StudentId
                    join TPCTable in _umsContext.TeacherPerCourses
                        on classTable.ClassId equals TPCTable.Id
                        join courseTable in _umsContext.Courses on TPCTable.CourseId equals courseTable.Id
                where courseTable.Id == request.courseId
                select userTable).Count();
            if (countStudents >= course.MaxStudentsNumber)
            {
                return "Course already full";
            }
            
            //check if in date range
            
                DateTime currentDate = DateTime.Now;
                DateOnly lowerBound = course.EnrolmentDateRange.Value.LowerBound;
                DateOnly upperBound = course.EnrolmentDateRange.Value.UpperBound;
                DateTime lowerBoundDateTime = lowerBound.ToDateTime(TimeOnly.MinValue);
                DateTime upperBoundDateTime = upperBound.ToDateTime(TimeOnly.MinValue);
                if (currentDate < lowerBoundDateTime || currentDate > upperBoundDateTime)
                {
                    return "out of date range";
                }
                else //Insert Student in Course
                {
                    ClassEnrollment classEnrollment = new ClassEnrollment();
                    classEnrollment.StudentId = request.studentId;
                    
                    classEnrollment.ClassId = classId;
                    _umsContext.ClassEnrollments.Add(classEnrollment);
                    _umsContext.SaveChanges();
                    ClassViewModel classViewModel = _mapper.Map<ClassViewModel>(classEnrollment);
                    
                    
                    
                    //send email to student
                    
                    string courseName = (from c in _umsContext.Courses
                        where c.Id == request.courseId
                        select c.Name).First(); //get course name
                    
                    string studentEmail = (from u in _umsContext.Users
                        where u.Id == request.studentId
                        select u.Email).First(); //get user email
                    
                    
                    string to = studentEmail; //To address    
                    string from = "johnny.asmar123@gmail.com"; //From address    
                    MailMessage message = new MailMessage(from, to);  
  
                    string mailbody = "You are enrolled in the course " + courseName + "." ;  
                    message.Subject = "Enrollnment";  
                    message.Body = mailbody;  
                    message.BodyEncoding = Encoding.UTF8;  
                    message.IsBodyHtml = true;  
                    SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
                    System.Net.NetworkCredential basicCredential1 = new  
                        System.Net.NetworkCredential("johnny.asmar123@gmail.com", "shhueyjpnpxerkag");  
                    client.EnableSsl = true;  
                    client.UseDefaultCredentials = false;  
                    client.Credentials = basicCredential1;  
                    try   
                    {  
                        client.Send(message);  
                    }   
  
                    catch (Exception ex)   
                    {  
                        throw ex;  
                    }  
                    
                    
                    
                    return "Student successfuly enrolled";
                }
            
        }
        

    }
}