using MediatR;
using PCP.Application.Exceptions;
using Persistence;

namespace PCP.Application.Entities.User.Commands.DeleteUser;

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, List<Domain.Models.User>>
{
    public UmsContext _umsContext;
    
    public DeleteUserHandler(UmsContext umsContext)
    {
        _umsContext = umsContext;
    }

    public async Task<List<Domain.Models.User>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        for (int i = 0; i < _umsContext.Users.ToList().Count; i++)
        {
            if (_umsContext.Users.ToList()[i].Id == request.Id)
            {
                _umsContext.Users.Remove(_umsContext.Users.ToList()[i]);
                _umsContext.SaveChanges();
                return _umsContext.Users.Select(x => x).ToList();
            }
        }
        throw new UserNotFound("User not found with this id");

        return null;
    }
    
}