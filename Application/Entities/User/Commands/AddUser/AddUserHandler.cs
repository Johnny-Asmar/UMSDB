using AutoMapper;
using MediatR;
using PCP.Application.ViewModel;
using Persistence;

namespace PCP.Application.Entities.User.Commands.AddUser;

public class AddUserHandler : IRequestHandler<AddUserCommand, UserViewModel>
{
    public UmsContext _umsContext;
    private IMapper _mapper;
    
    public AddUserHandler(UmsContext umsContext, IMapper mapper)
    {
        _umsContext = umsContext;
        _mapper = mapper;
    }

    public async Task<UserViewModel> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        Domain.Models.User user = new Domain.Models.User();
        user.Name = request.Name;
        user.RoleId = request.RoleId;
        user.KeycloakId = request.KeyClockId;
        user.Email = request.Email;
        user.SubsribeToEmail = request.SubscribeToEmail;
        _umsContext.Users.AddAsync(user);
        _umsContext.SaveChanges();
        
        UserViewModel userViewModel = _mapper.Map<UserViewModel>(user);
        return userViewModel;
    }
    
}