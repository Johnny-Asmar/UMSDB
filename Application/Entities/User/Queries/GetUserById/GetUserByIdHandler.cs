using MediatR;
using Persistence;

namespace PCP.Application.Entities.User.Queries.GetUserById;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, Domain.Models.User>
{
    public UmsContext _umsContext;
    
    public GetUserByIdHandler(UmsContext umsContext)
    {
        _umsContext = umsContext;
    }

    public async Task<Domain.Models.User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = (from c in _umsContext.Users
            where c.Id == request.Id
            select c).First();
        return user;
    }
}