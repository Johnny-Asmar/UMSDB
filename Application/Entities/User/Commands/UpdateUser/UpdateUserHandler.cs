using MediatR;
using PCP.Application.Exceptions;
using Persistence;

namespace PCP.Application.Entities.User.Commands.UpdateUser;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, List<Domain.Models.User>>
{
    private UmsContext _umsContext;
    
    public UpdateUserHandler(UmsContext umsContext)
    {
        _umsContext = umsContext;
    }

    public async Task<List<Domain.Models.User>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = _umsContext.Users.Where(u => u.Id == request.Id)
            .FirstOrDefault<Domain.Models.User>();
        if (existingUser != null)
        {
            existingUser.Name = request.name;
            return _umsContext.Users.ToList();
        }
        else
        {
            throw new UserNotFound("User not found with this id");
        }
    }
}