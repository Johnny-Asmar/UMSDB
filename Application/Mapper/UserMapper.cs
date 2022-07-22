using AutoMapper;
using Domain.Models;
using PCP.Application.ViewModel;

namespace PCP.Application.Mapper;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<User, UserViewModel>();
    }
}