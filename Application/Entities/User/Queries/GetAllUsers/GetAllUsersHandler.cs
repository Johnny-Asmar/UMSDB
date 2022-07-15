using MediatR;
using Persistence;

namespace PCP.Application.Entities.User.Queries.GetAllUsers;

public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, List<Domain.Models.User>>
{
    private UmsContext _umsContext ;
    
    public GetAllUsersHandler(UmsContext umsContext)
    {
        _umsContext = umsContext;
    }

    public async Task<List<Domain.Models.User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        List<Domain.Models.User> users = _umsContext.Users.Select(x => x).ToList();
        return users;
    }
    
}