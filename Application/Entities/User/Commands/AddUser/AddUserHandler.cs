using MediatR;
using Persistence;

namespace PCP.Application.Entities.User.Commands.AddUser;

public class AddUserHandler : IRequestHandler<AddUserCommand, List<Domain.Models.User>>
{
    public UmsContext _umsContext;
    
    public AddUserHandler(UmsContext umsContext)
    {
        _umsContext = umsContext;
    }

    public async Task<List<Domain.Models.User>> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        Domain.Models.User user = new Domain.Models.User();
        user.Name = request.Name;
        user.RoleId = request.RoleId;
        user.KeycloakId = request.KeyClockId;
        user.Email = request.Email;
        user.SubsribeToEmail = request.SubscribeToEmail;
        _umsContext.Users.AddAsync(user);
        _umsContext.SaveChanges();
        return _umsContext.Users.Select(x => x).ToList();
    }
    
}