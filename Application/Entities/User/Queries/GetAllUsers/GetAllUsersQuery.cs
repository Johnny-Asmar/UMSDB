using MediatR;

namespace PCP.Application.Entities.User.Queries.GetAllUsers;

public class GetAllUsersQuery: IRequest<List<Domain.Models.User>>
{
    
}