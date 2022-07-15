using AutoMapper;
using Domain.Models;
using PCP.Application.ViewModel;

namespace PCP.Application.Mapper;

public class ClassMapper : Profile
{
    public ClassMapper()
    {
        CreateMap<ClassEnrollment, ClassViewModel>();
    }
}