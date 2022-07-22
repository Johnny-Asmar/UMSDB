using MediatR;
using PCP.Application.ViewModel;

namespace PCP.Application.Entities.User.Queries.GetUserById;

public class GetUserByIdQuery : IRequest<UserViewModel>
{
    public int Id { get; set; }
}