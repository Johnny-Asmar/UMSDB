using AutoMapper;
using PCP.Application.ViewModel;

namespace PCP.Application.Mapper;

public class CourseMapper : Profile
{
    public CourseMapper()
    {
        CreateMap<Domain.Models.Course, CourseViewModel>()
            .ForMember(dest => dest.startTime, opt => opt.MapFrom(src => src.EnrolmentDateRange.Value.LowerBound))
            .ForMember(dest => dest.endTime, opt => opt.MapFrom(src => src.EnrolmentDateRange.Value.UpperBound));
            ;
    }
    
}