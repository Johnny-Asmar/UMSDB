using MediatR;

namespace PCP.Application.Entities.User.Commands.DeleteUser;

public class DeleteUserCommand : IRequest<List<Domain.Models.User>>
{
    public int Id { get; set; }
}