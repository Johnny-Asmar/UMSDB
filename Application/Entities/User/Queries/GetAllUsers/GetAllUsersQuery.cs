using MediatR;
using PCP.Application.ViewModel;

namespace PCP.Application.Entities.User.Queries.GetAllUsers;

public class GetAllUsersQuery: IRequest<List<UserViewModel>>
{
    
}