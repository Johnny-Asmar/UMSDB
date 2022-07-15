using MediatR;

namespace PCP.Application.Entities.User.Commands.AddUser;

public class AddUserCommand : IRequest<List<Domain.Models.User>>
{
    public Domain.Models.User user { get; set; }
}