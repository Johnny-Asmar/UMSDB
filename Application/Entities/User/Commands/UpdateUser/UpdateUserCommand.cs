using MediatR;

namespace PCP.Application.Entities.User.Commands.UpdateUser;

public class UpdateUserCommand : IRequest<List<Domain.Models.User>>
{
    public int Id { get; set; }
    public string name { get; set; }
}