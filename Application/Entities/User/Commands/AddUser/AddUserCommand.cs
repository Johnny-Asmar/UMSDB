using MediatR;

namespace PCP.Application.Entities.User.Commands.AddUser;

public class AddUserCommand : IRequest<List<Domain.Models.User>>
{
    public string Name { get; set; }
    public string KeyClockId { get; set; }
    public long RoleId { get; set; }
    public string Email { get; set; }
    public int SubscribeToEmail { get; set; }

}