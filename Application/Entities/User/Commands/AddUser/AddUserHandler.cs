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
        _umsContext.Users.AddAsync(request.user);
        _umsContext.SaveChanges();
        return _umsContext.Users.Select(x => x).ToList();
    }
    
}