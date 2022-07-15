using AutoMapper;
using PCP.Application.ViewModel;

namespace PCP.Application.Mapper;

public class CourseMapper : Profile
{
    public CourseMapper()
    {
        CreateMap<Domain.Models.Course, CourseViewModel>();
    }
    
}